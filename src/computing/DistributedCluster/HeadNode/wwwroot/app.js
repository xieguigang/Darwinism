// 仪表盘前端：每 1.5 秒轮询 /api/status 并局部刷新。
(() => {
    const POLL_MS = 1500;
    const $ = (id) => document.getElementById(id);

    // 节点监控视图模式：'full' 完整视图 / 'heatmap' 热图模式。
    let monitorMode = 'full';
    // 算力热图指标：'used' 已用算力 / 'free' 剩余算力。
    let heatMetric = 'used';
    // 当前热图选中的节点 id（点击方格后用于右侧详情卡片）。
    let selectedNodeId = null;
    // 历史采样环形缓冲：Map<nodeId, {time:[], cpu:[], mem:[]}>，每节点最多保留 MAX_POINTS 个点。
    const history = new Map();
    const MAX_POINTS = 40;
    let historyChart = null;       // echarts 实例
    let historyInited = false;    // 图表是否已初始化（用于 resize 优化）
    let lastStatus = null;        // 最近一次状态快照，供主题切换重绘热图使用

    // 集群在线/离线状态机：连续 N 次 fetch 失败（多为管理节点离线）后切离线，恢复时提示上线。
    const OFFLINE_THRESHOLD = 5;
    let fetchFailStreak = 0;       // 连续 "Failed to fetch" 次数
    let clusterOffline = false;    // 当前是否处于离线状态

    // 当前计算任务名称（前端记录最近一次成功提交的任务；刷新后从 localStorage 恢复）。
    let currentTaskName = (() => { try { return localStorage.getItem('current-task-name') || ''; } catch (e) { return ''; } })();

    /* ============ 右下角 Toast 提示 ============ */
    const TOAST_ICONS = { info: 'ℹ', success: '✓', warn: '⚠', error: '✕' };

    function showToast(message, type = 'info', duration = 3500) {
        const container = document.getElementById('toastContainer');
        if (!container) return;
        const t = String(type).toLowerCase();
        const el = document.createElement('div');
        el.className = 'toast ' + t;
        el.setAttribute('role', 'status');

        const icon = document.createElement('span');
        icon.className = 'toast-icon';
        icon.textContent = TOAST_ICONS[t] || TOAST_ICONS.info;

        const msg = document.createElement('span');
        msg.className = 'toast-msg';
        msg.textContent = message;

        const close = document.createElement('button');
        close.className = 'toast-close';
        close.type = 'button';
        close.setAttribute('aria-label', '关闭');
        close.textContent = '×';

        let timer = null;
        const dismiss = () => {
            if (el.classList.contains('leaving')) return;
            el.classList.add('leaving');
            el.addEventListener('animationend', () => el.remove(), { once: true });
            // 兜底：动画未触发时也能移除
            setTimeout(() => el.remove(), 400);
        };
        close.addEventListener('click', dismiss);

        el.appendChild(icon);
        el.appendChild(msg);
        el.appendChild(close);
        container.appendChild(el);

        if (duration > 0) {
            timer = setTimeout(dismiss, duration);
            // 悬停暂停自动关闭
            el.addEventListener('mouseenter', () => { clearTimeout(timer); });
            el.addEventListener('mouseleave', () => { timer = setTimeout(dismiss, 1200); });
        }
    }

    /* ============ 主题切换（light / dark） ============ */
    const THEME_KEY = 'cluster-theme';
    let theme = localStorage.getItem(THEME_KEY) ||
        (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');

    function applyTheme(theme) {
        document.documentElement.setAttribute('data-theme', theme);
        const btn = document.getElementById('themeToggle');
        if (btn) {
            const isLight = theme === 'light';
            btn.querySelector('.theme-toggle-icon').textContent = isLight ? '☀' : '☾';
            btn.querySelector('.theme-toggle-label').textContent = isLight ? 'Light' : 'Dark';
            btn.setAttribute('title', isLight ? '切换到暗色主题' : '切换到亮色主题');
        }
    }

    function initThemeToggle() {
        // 默认 light；若用户之前手动切换过，则沿用其选择
        let saved = null;
        try { saved = localStorage.getItem(THEME_KEY); } catch (e) { /* localStorage 不可用则忽略 */ }
        const initial = saved === 'dark' || saved === 'light' ? saved : 'light';
        applyTheme(initial);

        const btn = document.getElementById('themeToggle');
        if (btn) {
            btn.addEventListener('click', () => {
                const next = document.documentElement.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
                applyTheme(next);
                theme = next;   // 同步模块级主题，供热图/图表配色使用
                try { localStorage.setItem(THEME_KEY, next); } catch (e) { /* 忽略 */ }
                showToast('已切换至 ' + (next === 'dark' ? '暗色' : '亮色') + '主题', 'info', 2000);
                // 主题切换后重绘图表与热图（若当前可见）
                if (historyInited) renderHistoryChart();
                if (monitorMode === 'heatmap' && lastStatus) renderHeatmap(lastStatus);
            });
        }
    }

    function fmtTime(ticks) {
        if (!ticks) return '--';
        const d = new Date(ticks / 10000 - 62135596800000);
        return d.toLocaleTimeString();
    }

    function escapeHtml(s) {
        if (!s) return '';
        return s.replace(/[&<>"']/g, (c) => ({
            '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;'
        }[c]));
    }

    async function fetchStatus() {
        const res = await fetch('/api/status');
        if (!res.ok) throw new Error('HTTP ' + res.status);
        return await res.json();
    }

    // 更新集群在线/离线状态（顶栏徽标 + body 类）。
    function setClusterOffline(offline) {
        document.body.classList.toggle('cluster-offline', offline);
        const chip = document.getElementById('onlineChip');
        if (chip) {
            if (offline) {
                chip.textContent = '管理节点离线';
                chip.classList.add('offline');
            } else {
                chip.textContent = '在线节点 ' + (lastStatus ? lastStatus.onlineNodes : 0);
                chip.classList.remove('offline');
            }
        }
        // 管理节点离线时禁用【提交任务】按钮，防止触发弹窗；上线后恢复可点击。
        const submitBtn = document.getElementById('btnOpenSubmit');
        if (submitBtn) {
            submitBtn.disabled = offline;
            submitBtn.title = offline ? '管理节点离线，暂不可提交任务' : '提交计算任务';
        }
    }

    // 更新顶栏当前任务状态（无任务显示空闲，有任务显示名称）。
    function renderTaskChip() {
        const dot = document.getElementById('taskDot');
        const label = document.getElementById('taskLabel');
        if (!label) return;
        if (currentTaskName) {
            label.textContent = '运行中：' + currentTaskName;
            if (dot) dot.className = 'task-status-dot running';
        } else {
            label.textContent = '集群空闲';
            if (dot) dot.className = 'task-status-dot idle';
        }
    }

    function renderOverview(s) {
        $('clusterName').textContent = s.clusterName || 'Darwinism Cluster';
        $('onlineChip').textContent = '在线节点 ' + s.onlineNodes;
        $('mTotalNodes').textContent = (s.nodes || []).length;
        $('mOnline').textContent = s.onlineNodes;
        $('mCores').textContent = s.totalCores;
        // 物理内存总和：优先使用后端已汇总值，缺失时降级为前端对节点求和。
        const memMB = Number(s.totalMemoryMB) > 0
            ? Number(s.totalMemoryMB)
            : (s.nodes || []).reduce((acc, n) => acc + (Number(n.totalMemoryMB) || 0), 0);
        $('mMemory').textContent = fmtMemTotal(memMB);
        $('mPower').textContent = s.powerIndex;
        $('clock').textContent = fmtTime(s.serverTime);

        $('mDone').textContent = s.completedBlocks;
        $('mRun').textContent = s.runningBlocks;
        $('mPending').textContent = s.pendingBlocks;
        $('mFailed').textContent = s.failedBlocks;

        const total = s.completedBlocks + s.runningBlocks + s.pendingBlocks + s.failedBlocks;
        const pct = (n) => (total === 0 ? 0 : (n / total) * 100) + '%';
        $('segDone').style.width = pct(s.completedBlocks);
        $('segRun').style.width = pct(s.runningBlocks);
        $('segPending').style.width = pct(s.pendingBlocks);
        $('segFailed').style.width = pct(s.failedBlocks);

        const failRate = total === 0 ? 0 : ((s.failedBlocks / (s.completedBlocks + s.failedBlocks)) * 100).toFixed(1);
        $('failRate').textContent = failRate + '%';
    }

    // 将字节/秒格式化为可读速率（KB/s、MB/s）。
    function fmtRate(bytesPerSec) {
        const v = Number(bytesPerSec) || 0;
        if (v <= 0) return '0 B/s';
        if (v < 1024) return v.toFixed(0) + ' B/s';
        if (v < 1024 * 1024) return (v / 1024).toFixed(1) + ' KB/s';
        return (v / 1024 / 1024).toFixed(2) + ' MB/s';
    }

    // 将 MB 数值格式化为可读内存（GB 优先）。
    function fmtMemMB(mb) {
        const v = Number(mb) || 0;
        if (v >= 1024) return (v / 1024).toFixed(1) + ' GB';
        return v.toFixed(0) + ' MB';
    }

    // 将集群总内存(MB)格式化为 GB / TB，≥1024 GB 时自动切换为 TB（保留 2 位小数）。
    function fmtMemTotal(mb) {
        const v = Number(mb) || 0;
        const gb = v / 1024;
        if (gb >= 1024) return (gb / 1024).toFixed(2) + ' TB';
        return gb.toFixed(1) + ' GB';
    }

    // 按使用率返回进度条配色类（绿/黄/红）。
    function usageClass(pct) {
        if (pct >= 85) return 'bar-danger';
        if (pct >= 60) return 'bar-warn';
        return 'bar-ok';
    }

    function renderNodes(s) {
        const list = $('nodeList');
        const nodes = s.nodes || [];
        if (nodes.length === 0) {
            list.innerHTML = '<div class="empty">暂无节点上报</div>';
            return;
        }
        list.innerHTML = nodes.map((n) => {
            const cpu = Math.max(0, Math.min(100, Number(n.cpuUsage) || 0));
            const mem = Math.max(0, Math.min(100, Number(n.memoryUsage) || 0));
            const name = escapeHtml(n.machineName || n.nodeId || '—');
            const ip = escapeHtml(n.ipAddress || '—');
            const block = n.currentBlock
                ? '计算中 · ' + escapeHtml(n.currentBlock)
                : '空闲';
            const statusCls = n.online ? 'status-online' : 'status-offline';
            const statusText = n.online ? '在线' : '失联';

            return `
            <div class="node-card ${n.online ? '' : 'node-offline'}">
                <div class="node-head">
                    <div class="node-id">
                        <div class="node-name">${name}</div>
                        <div class="node-ip">${ip}</div>
                    </div>
                    <div class="node-state">
                        <span class="status-dot ${statusCls}"></span>
                        <span class="state-text">${statusText}</span>
                    </div>
                </div>

                <div class="node-tags">
                    <span class="tag">${n.cores || 0} 核</span>
                    <span class="tag">${block}</span>
                </div>

                <div class="meter">
                    <div class="meter-label"><span>CPU 使用率</span><span>${cpu.toFixed(1)}%</span></div>
                    <div class="meter-bar"><div class="meter-fill ${usageClass(cpu)}" style="width:${cpu}%"></div></div>
                </div>

                <div class="meter">
                    <div class="meter-label"><span>内存使用率</span><span>${mem.toFixed(1)}% · ${fmtMemMB(n.totalMemoryMB)}</span></div>
                    <div class="meter-bar"><div class="meter-fill ${usageClass(mem)}" style="width:${mem}%"></div></div>
                </div>

                <div class="net">
                    <span class="net-up">↑ ${fmtRate(n.netUploadRate)}</span>
                    <span class="net-down">↓ ${fmtRate(n.netDownloadRate)}</span>
                </div>
            </div>`;
        }).join('');
    }

    function renderFailures(s) {
        const list = $('failureList');
        const fails = s.failures || [];
        if (fails.length === 0) {
            list.innerHTML = '<div class="empty">无失败数据块</div>';
            return;
        }
        list.innerHTML = fails.map((f) => `
            <div class="failure-item" onclick="this.classList.toggle('open')">
                <div class="fid">${escapeHtml(f.blockId)}</div>
                <div class="block">重试 ${f.retryCount} 次 · ${escapeHtml(f.message || '')}</div>
                <div class="failure-detail">${escapeHtml((f.stackTrace || '') + '\n\n[log] ' + (f.logPath || ''))}</div>
            </div>`).join('');
    }

    // 将 0-100 的使用率映射为「绿→黄→红」渐变（HSL 色相 120→0）。
    function heatColor(pct) {
        const p = Math.max(0, Math.min(100, Number(pct) || 0));
        const hue = 120 - (p / 100) * 120;        // 120 绿 → 0 红
        const light = theme === 'dark' ? 42 : 46;  // 暗色稍暗，保证文字可读
        return `hsl(${hue.toFixed(0)}, 70%, ${light}%)`;
    }

    // 按后端算力指数公式（@Scheduler.vb:273）计算单节点的算力指数：
    //   node_total = Sqrt( (cores/REF_CORES) * ((totalMemoryMB/1024)/REF_MEM_GB) ) * 100
    //   node_used  = Sqrt( (usedCores/REF_CORES) * ((usedMemMB/1024)/REF_MEM_GB) ) * 100
    //   node_free  = node_total - node_used
    const REF_CORES = 64;
    const REF_MEM_GB = 256;
    function computeNodePower(n) {
        const cores = Math.max(0, Number(n.cores) || 0);
        const totalMem = Math.max(0, Number(n.totalMemoryMB) || 0);
        const cpuU = Math.max(0, Math.min(100, Number(n.cpuUsage) || 0)) / 100;
        const memU = Math.max(0, Math.min(100, Number(n.memoryUsage) || 0)) / 100;
        const usedCores = cores * cpuU;
        const usedMem = totalMem * memU;
        const total = Math.sqrt((cores / REF_CORES) * ((totalMem / 1024) / REF_MEM_GB)) * 100;
        const used = Math.sqrt((usedCores / REF_CORES) * ((usedMem / 1024) / REF_MEM_GB)) * 100;
        const free = Math.max(0, total - used);
        return { total: Math.round(total), used: Math.round(used), free: Math.round(free) };
    }

    // 渲染热图模式：计算节点「算力指数」热图（已用 / 剩余 两种指标可切换）。
    // 每个节点显示一个方格，鼠标点击后于右侧显示该节点完整信息卡片。
    function renderHeatmap(s) {
        const nodes = (s.nodes || []).filter((n) => n.online);
        const box = $('heatPower');
        if (!box) return;

        // 标题随指标切换
        const title = $('heatTitle');
        if (title) title.textContent = '节点算力指数热图（' + (heatMetric === 'used' ? '已用算力' : '剩余算力') + '）';

        if (nodes.length === 0) {
            box.innerHTML = '<div class="empty">暂无在线节点</div>';
            return;
        }

        box.innerHTML = nodes.map((n) => {
            const p = computeNodePower(n);
            const val = heatMetric === 'used' ? p.used : p.free;
            // 颜色强度按相对其总算力的占比映射
            const ratio = p.total > 0 ? val / p.total : 0;
            const color = heatColor(Math.min(100, ratio * 100));
            const id = n.nodeId || n.machineName;
            const isSel = id === selectedNodeId ? ' is-selected' : '';
            const name = escapeHtml(n.machineName || n.nodeId || '—');
            const tip = [
                name + '  (' + escapeHtml(n.ipAddress || '—') + ')',
                '总算力指数：' + p.total,
                '已用算力：' + p.used,
                '剩余算力：' + p.free,
                'CPU 使用率：' + (Number(n.cpuUsage) || 0).toFixed(1) + '%',
                '内存使用率：' + (Number(n.memoryUsage) || 0).toFixed(1) + '% · ' + fmtMemMB(n.totalMemoryMB),
                '逻辑核心：' + (n.cores || 0),
                '网络：↑ ' + fmtRate(n.netUploadRate) + ' / ↓ ' + fmtRate(n.netDownloadRate),
                '当前任务：' + (n.currentBlock ? '计算中 · ' + escapeHtml(n.currentBlock) : '空闲')
            ].join('\n');
            const sub = heatMetric === 'used' ? '已用 ' + p.used : '剩余 ' + p.free;
            return `<div class="heat-cell${isSel}" data-nodeid="${id}" style="background:${color}" title="${tip.replace(/"/g, '&quot;')}">
                        <span class="hc-name">${name}</span>
                        <span class="hc-val">${val}</span>
                        <span class="hc-sub">${sub} / 共${p.total}</span>
                    </div>`;
        }).join('');
    }

    // 右侧节点详情卡片：展示被选中的计算节点完整信息。
    function renderNodeDetail(n) {
        const panel = $('nodeDetail');
        const body = $('nodeDetailBody');
        if (!panel || !body) return;
        if (!n) { panel.hidden = true; return; }
        const p = computeNodePower(n);
        const cpu = Math.max(0, Math.min(100, Number(n.cpuUsage) || 0));
        const mem = Math.max(0, Math.min(100, Number(n.memoryUsage) || 0));
        const usedPct = p.total > 0 ? (p.used / p.total * 100) : 0;
        const fillColor = heatColor(Math.min(100, usedPct));
        body.innerHTML = `
            <div class="nd-row">
                <span class="nd-k">节点名称</span>
                <span class="nd-v">${escapeHtml(n.machineName || n.nodeId || '—')}</span>
            </div>
            <div class="nd-row">
                <span class="nd-k">IP 地址</span>
                <span class="nd-v">${escapeHtml(n.ipAddress || '—')}</span>
            </div>
            <div class="nd-row">
                <span class="nd-k">运行状态</span>
                <span class="nd-v accent">${n.online ? '在线' : '失联'}</span>
            </div>
            <div class="nd-row">
                <span class="nd-k">算力指数（总 / 已用 / 剩余）</span>
                <span class="nd-v accent">${p.total} · ${p.used} · ${p.free}</span>
                <div class="nd-power-bar"><div class="nd-power-fill" style="width:${usedPct.toFixed(1)}%;background:${fillColor}"></div></div>
            </div>
            <div class="nd-row">
                <span class="nd-k">CPU 使用率</span>
                <span class="nd-v">${cpu.toFixed(1)}%</span>
            </div>
            <div class="nd-row">
                <span class="nd-k">内存使用率</span>
                <span class="nd-v">${mem.toFixed(1)}% · ${fmtMemMB(n.totalMemoryMB)}</span>
            </div>
            <div class="nd-row">
                <span class="nd-k">逻辑核心数</span>
                <span class="nd-v">${n.cores || 0}</span>
            </div>
            <div class="nd-row">
                <span class="nd-k">网络流量</span>
                <span class="nd-v">↑ ${fmtRate(n.netUploadRate)} / ↓ ${fmtRate(n.netDownloadRate)}</span>
            </div>
            <div class="nd-row">
                <span class="nd-k">当前任务</span>
                <span class="nd-v">${n.currentBlock ? '计算中 · ' + escapeHtml(n.currentBlock) : '空闲'}</span>
            </div>`;
        panel.hidden = false;
    }

    // 每轮轮询把各节点当前 CPU / 内存使用率追加进环形缓冲。
    function sampleHistory(s) {
        const t = fmtTime(s.serverTime);
        for (const n of (s.nodes || [])) {
            if (!n.online) continue;
            const id = n.nodeId || n.machineName;
            if (!history.has(id)) history.set(id, { time: [], cpu: [], mem: [], name: n.machineName || id });
            const buf = history.get(id);
            buf.time.push(t);
            buf.cpu.push(+(Number(n.cpuUsage) || 0).toFixed(1));
            buf.mem.push(+(Number(n.memoryUsage) || 0).toFixed(1));
            if (buf.time.length > MAX_POINTS) {
                buf.time.shift(); buf.cpu.shift(); buf.mem.shift();
            }
        }
    }

    // 当前主题下的图表配色（坐标轴 / 文本 / 网格）。
    function chartPalette() {
        return {
            text: theme === 'dark' ? '#e6e6e6' : '#1a1a1a',
            subtle: theme === 'dark' ? '#9aa0a6' : '#666',
            grid: theme === 'dark' ? 'rgba(255,255,255,.08)' : 'rgba(0,0,0,.07)'
        };
    }

    // 初始化 echarts 历史曲线实例（仅一次）。
    function ensureHistoryChart() {
        if (historyInited || typeof echarts === 'undefined') return;
        const el = $('historyChart');
        if (!el) return;
        historyChart = echarts.init(el, theme === 'dark' ? 'dark' : null);
        historyInited = true;
    }

    // 渲染历史曲线：每个节点两条 series（CPU 实线 / 内存虚线）。
    function renderHistoryChart() {
        if (!historyChart) return;
        const pal = chartPalette();
        const series = [];
        const legend = [];
        // 颜色调色板（按节点循环取色）
        const colors = ['#0a84ff', '#ca5010', '#107c10', '#d32f2f', '#8e44ad', '#16a085', '#e67e22', '#2980b9'];
        let ci = 0;
        for (const [id, buf] of history) {
            const color = colors[ci % colors.length]; ci++;
            const label = buf.name || id;
            legend.push(label + ' CPU', label + ' 内存');
            series.push({
                name: label + ' CPU', type: 'line', showSymbol: false, smooth: true,
                lineStyle: { width: 1.8, color: color }, itemStyle: { color: color },
                data: buf.cpu, emphasis: { focus: 'series' }
            });
            series.push({
                name: label + ' 内存', type: 'line', showSymbol: false, smooth: true,
                lineStyle: { width: 1.8, color: color, type: 'dashed' }, itemStyle: { color: color },
                data: buf.mem, emphasis: { focus: 'series' }
            });
        }
        const xData = Array.from(history.values())[0]?.time || [];
        historyChart.setOption({
            color: colors,
            tooltip: { trigger: 'axis', backgroundColor: pal.text === '#e6e6e6' ? 'rgba(30,30,30,.95)' : 'rgba(255,255,255,.96)',
                borderColor: pal.grid, textStyle: { color: pal.text } },
            legend: { type: 'scroll', textStyle: { color: pal.subtle }, top: 0, data: legend },
            grid: { left: 44, right: 18, top: 36, bottom: 30 },
            xAxis: { type: 'category', data: xData, boundaryGap: false,
                axisLine: { lineStyle: { color: pal.grid } },
                axisLabel: { color: pal.subtle, fontSize: 10 } },
            yAxis: { type: 'value', min: 0, max: 100, name: '使用率 %', nameTextStyle: { color: pal.subtle },
                axisLabel: { color: pal.subtle, formatter: '{value}%' },
                splitLine: { lineStyle: { color: pal.grid } } },
            series: series
        }, { notMerge: false });
    }

    function renderLogs(s) {
        const logs = s.logs || [];
        $('logStream').textContent = logs.length ? logs.join('\n') : '等待数据…';
        const el = $('logStream');
        el.scrollTop = el.scrollHeight;
    }

    function renderMeta(s) {
        $('smbRoot').textContent = s.smbRoot;
        $('httpPort').textContent = s.httpPort;
        $('poll').textContent = s.pollInterval;
        $('totalJobs').textContent = s.totalJobs;
    }

    async function tick() {
        try {
            const s = await fetchStatus();
            lastStatus = s;

            // 恢复上线：从离线状态恢复时提示，并重置失败计数
            if (clusterOffline) {
                clusterOffline = false;
                fetchFailStreak = 0;
                setClusterOffline(false);
                showToast('集群已上线，状态已恢复同步', 'success', 3500);
            }
            fetchFailStreak = 0;

            renderOverview(s);
            renderNodes(s);
            renderFailures(s);
            renderLogs(s);
            renderMeta(s);
            renderTaskChip();

            // 视图模式切换显示
            const isHeat = monitorMode === 'heatmap';
            $('nodeList').hidden = isHeat;
            $('heatLayout').hidden = !isHeat;
            if (isHeat) {
                renderHeatmap(s);
                // 若已选中某节点，刷新其详情（数据可能更新）
                if (selectedNodeId) {
                    const sel = (s.nodes || []).find((n) => (n.nodeId || n.machineName) === selectedNodeId);
                    renderNodeDetail(sel || null);
                }
            } else {
                $('nodeDetail').hidden = true;
            }

            // 历史采样 + 曲线（仅当前已渲染图表时更新）
            sampleHistory(s);
            if (historyInited) {
                ensureHistoryChart();
                renderHistoryChart();
            }
        } catch (e) {
            console.warn('状态轮询失败：', e.message);
            // 仅当为网络层 fetch 失败（管理节点离线可能性高）时累计；其余错误正常提示。
            const isFetchFail = /Failed to fetch/i.test(e.message);
            if (isFetchFail) {
                fetchFailStreak++;
                if (fetchFailStreak >= OFFLINE_THRESHOLD && !clusterOffline) {
                    clusterOffline = true;
                    setClusterOffline(true);
                    showToast('管理节点已离线，停止状态刷新', 'error', 5000);
                }
                // 离线后不再重复弹出 "Failed to fetch"，避免刷屏
                return;
            }
            showToast('状态轮询失败：' + e.message, 'error', 3000);
        }
    }

    // ============ 任务提交（模态框） ============
    const submitModal = document.getElementById('submitModal');
    const btnOpen = document.getElementById('btnOpenSubmit');

    function openSubmitModal() {
        if (submitModal) submitModal.hidden = false;
        document.body.classList.add('modal-open');
    }
    function closeSubmitModal() {
        if (submitModal) submitModal.hidden = true;
        document.body.classList.remove('modal-open');
        const out = document.getElementById('submitResult');
        if (out) out.textContent = '';
    }

    if (btnOpen) btnOpen.addEventListener('click', openSubmitModal);
    const btnClose = document.getElementById('btnCloseSubmit');
    const btnCancel = document.getElementById('btnCancelSubmit');
    if (btnClose) btnClose.addEventListener('click', closeSubmitModal);
    if (btnCancel) btnCancel.addEventListener('click', closeSubmitModal);
    // 点击遮罩空白区域关闭
    if (submitModal) {
        submitModal.addEventListener('click', (e) => {
            if (e.target === submitModal) closeSubmitModal();
        });
    }
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape' && submitModal && !submitModal.hidden) closeSubmitModal();
    });

    // ---------- 通用：文件树懒加载 ----------
    // 根据 dir(相对 webRoot) 拉取一层子节点，返回 FileNode[]
    async function fetchTree(dir) {
        const res = await fetch('/api/files/tree?dir=' + encodeURIComponent(dir || ''));
        if (!res.ok) return [];
        try { return await res.json(); } catch (e) { return []; }
    }

    // 渲染一行文件树节点（目录可展开，dll 可点选）
    function renderNode(container, node, kind) {
        const row = document.createElement('div');
        row.className = 'tree-row' + (node.isDir ? ' is-dir' : '') + (node.isDll ? ' is-dll' : '');

        const icon = document.createElement('span');
        icon.className = 'tree-icon';
        icon.textContent = node.isDir ? '📁' : (node.isDll ? '🧩' : '📄');
        row.appendChild(icon);

        const label = document.createElement('span');
        label.className = 'tree-label';
        label.textContent = node.name;
        row.appendChild(label);

        if (node.isDir) {
            if (node.hasDataset) {
                const tag = document.createElement('span');
                tag.className = 'tree-tag dataset-tag';
                tag.textContent = 'dataset';
                row.appendChild(tag);
            } else if (node.hasDllChildren) {
                const tag = document.createElement('span');
                tag.className = 'tree-tag';
                tag.textContent = 'dll';
                row.appendChild(tag);
            }
            const caret = document.createElement('span');
            caret.className = 'tree-caret';
            caret.textContent = '▸';
            row.prepend(caret);

            let expanded = false;
            let childBox = null;

            row.addEventListener('click', async () => {
                if (!expanded) {
                    expanded = true;
                    caret.textContent = '▾';
                    childBox = document.createElement('div');
                    childBox.className = 'tree-children';
                    row.after(childBox);
                    const spinner = document.createElement('div');
                    spinner.className = 'tree-row loading-row';
                    spinner.innerHTML = '<span class="spinner"></span><span class="tree-label">加载中…</span>';
                    childBox.appendChild(spinner);
                    try {
                        const children = await fetchTree(node.fullPath);
                        spinner.remove();
                        if (children.length === 0) {
                            const empty = document.createElement('div');
                            empty.className = 'tree-empty';
                            empty.textContent = '空目录';
                            childBox.appendChild(empty);
                        }
                        children
                            .slice()
                            .sort((a, b) => (a.isDir === b.isDir) ? a.name.localeCompare(b.name) : (a.isDir ? -1 : 1))
                            .forEach(c => renderNode(childBox, c, kind));
                    } catch (e) {
                        spinner.remove();
                        const err = document.createElement('div');
                        err.className = 'tree-empty';
                        err.textContent = '加载失败';
                        childBox.appendChild(err);
                    }
                } else {
                    expanded = false;
                    caret.textContent = '▸';
                    if (childBox) { childBox.remove(); childBox = null; }
                }
            });
        } else if (node.isDll && kind === 'dll') {
            row.addEventListener('click', () => {
                document.getElementById('inAssembly').value = node.fullPath;
                document.querySelectorAll('#dllTree .tree-row.selected').forEach(r => r.classList.remove('selected'));
                row.classList.add('selected');
                loadAssemblyMethods(node.fullPath);
            });
        } else if (!node.isDir && kind === 'data') {
            row.addEventListener('click', () => {
                document.querySelectorAll('#dataTree .tree-row.selected').forEach(r => r.classList.remove('selected'));
                row.classList.add('selected');
            });
        }

        // 数据目录树：点击目录即选定数据源目录（用于 dataset 预览）
        if (node.isDir && kind === 'data') {
            const pick = document.createElement('button');
            pick.className = 'tree-pick';
            pick.textContent = '选择';
            pick.title = '选定此目录作为数据输入源';
            pick.addEventListener('click', (e) => {
                e.stopPropagation();
                document.getElementById('inDatasetDir').value = node.fullPath;
                document.querySelectorAll('#dataTree .tree-row.picked').forEach(r => r.classList.remove('picked'));
                row.classList.add('picked');
                previewDataset(node.fullPath);
            });
            row.appendChild(pick);
        }

        container.appendChild(row);
    }

    function initTree(treeId, kind) {
        const box = document.getElementById(treeId);
        if (!box) return;
        box.innerHTML = '';
        const root = document.createElement('div');
        root.className = 'tree-row is-dir';
        root.innerHTML = '<span class="tree-caret">▸</span><span class="tree-icon">📁</span><span class="tree-label">web 根目录</span>';
        box.appendChild(root);
        let expanded = false, childBox = null;
        root.addEventListener('click', async () => {
            if (!expanded) {
                expanded = true;
                root.querySelector('.tree-caret').textContent = '▾';
                childBox = document.createElement('div');
                childBox.className = 'tree-children';
                root.after(childBox);
                const spinner = document.createElement('div');
                spinner.className = 'tree-row loading-row';
                spinner.innerHTML = '<span class="spinner"></span><span class="tree-label">加载中…</span>';
                childBox.appendChild(spinner);
                try {
                    const children = await fetchTree('');
                    spinner.remove();
                    if (children.length === 0) {
                        const empty = document.createElement('div');
                        empty.className = 'tree-empty';
                        empty.textContent = '根目录为空';
                        childBox.appendChild(empty);
                    }
                    children
                        .slice()
                        .sort((a, b) => (a.isDir === b.isDir) ? a.name.localeCompare(b.name) : (a.isDir ? -1 : 1))
                        .forEach(c => renderNode(childBox, c, kind));
                } catch (e) {
                    spinner.remove();
                    const err = document.createElement('div');
                    err.className = 'tree-empty';
                    err.textContent = '加载失败';
                    childBox.appendChild(err);
                }
            } else {
                expanded = false;
                root.querySelector('.tree-caret').textContent = '▸';
                if (childBox) { childBox.remove(); childBox = null; }
            }
        });
    }

    // ---------- Assembly 方法扫描 ----------
    async function loadAssemblyMethods(path) {
        const tree = document.getElementById('methodTree');
        const hint = document.getElementById('methodHint');
        const doc = document.getElementById('methodDoc');
        if (!tree) return;
        tree.innerHTML = '<div class="tree-row loading-row"><span class="spinner"></span><span class="tree-label">扫描程序集…</span></div>';
        if (hint) hint.textContent = '扫描中：' + path;
        if (doc) doc.innerHTML = '<div class="doc-empty">加载中…</div>';

        try {
            const res = await fetch('/api/assembly/scan?assemblypath=' + encodeURIComponent(path));
            const data = await res.json();
            if (data.methods === undefined) {
                tree.innerHTML = '<div class="tree-empty">扫描失败：' + (data.message || '未知错误') + '</div>';
                if (hint) hint.textContent = '扫描失败';
                return;
            }
            const methods = data.methods || [];
            if (methods.length === 0) {
                tree.innerHTML = '<div class="tree-empty">未找到符合 worker 调用约定的方法</div>';
                if (hint) hint.textContent = '无可用方法';
                return;
            }
            // 按 namespace -> class -> method 构建对象树
            const nsMap = {};
            methods.forEach(m => {
                nsMap[m.namespace] = nsMap[m.namespace] || {};
                nsMap[m.namespace][m.class] = nsMap[m.namespace][m.class] || [];
                nsMap[m.namespace][m.class].push(m);
            });
            tree.innerHTML = '';
            Object.keys(nsMap).sort().forEach(ns => {
                const nsRow = document.createElement('div');
                nsRow.className = 'tree-row is-dir';
                nsRow.innerHTML = '<span class="tree-caret">▾</span><span class="tree-icon">📦</span><span class="tree-label"></span>';
                nsRow.querySelector('.tree-label').textContent = ns || '(无命名空间)';
                tree.appendChild(nsRow);
                const nsBox = document.createElement('div');
                nsBox.className = 'tree-children';
                nsRow.after(nsBox);
                nsRow.addEventListener('click', () => {
                    const open = nsBox.style.display !== 'none';
                    nsBox.style.display = open ? 'none' : '';
                    nsRow.querySelector('.tree-caret').textContent = open ? '▸' : '▾';
                });
                Object.keys(nsMap[ns]).sort().forEach(cls => {
                    const clsRow = document.createElement('div');
                    clsRow.className = 'tree-row is-dir';
                    clsRow.innerHTML = '<span class="tree-caret">▾</span><span class="tree-icon">🧱</span><span class="tree-label"></span>';
                    clsRow.querySelector('.tree-label').textContent = cls;
                    nsBox.appendChild(clsRow);
                    const clsBox = document.createElement('div');
                    clsBox.className = 'tree-children';
                    clsRow.after(clsBox);
                    clsRow.addEventListener('click', () => {
                        const open = clsBox.style.display !== 'none';
                        clsBox.style.display = open ? 'none' : '';
                        clsRow.querySelector('.tree-caret').textContent = open ? '▸' : '▾';
                    });
                    nsMap[ns][cls].forEach(m => {
                        const mRow = document.createElement('div');
                        mRow.className = 'tree-row is-method';
                        mRow.innerHTML = '<span class="tree-icon">⚡</span><span class="tree-label"></span>';
                        mRow.querySelector('.tree-label').textContent = m.method;
                        clsBox.appendChild(mRow);
                        mRow.addEventListener('click', (e) => {
                            e.stopPropagation();
                            document.querySelectorAll('#methodTree .tree-row.selected').forEach(r => r.classList.remove('selected'));
                            mRow.classList.add('selected');
                            document.getElementById('inMethod').value = m.signature.replace(/\(.*\)$/, '');
                            showMethodDoc(m);
                        });
                    });
                });
            });
            if (hint) hint.textContent = '共 ' + methods.length + ' 个可用方法';
        } catch (e) {
            tree.innerHTML = '<div class="tree-empty">扫描异常：' + e.message + '</div>';
            if (hint) hint.textContent = '扫描异常';
        }
    }

    function showMethodDoc(m) {
        const doc = document.getElementById('methodDoc');
        if (!doc) return;
        const sig = document.createElement('div');
        sig.className = 'doc-sig';
        sig.textContent = m.signature;
        const sum = document.createElement('div');
        sum.className = 'doc-summary';
        sum.innerHTML = '<span class="doc-key">summary</span>' + escapeHtml(m.summary || '(无注释)');
        const rem = document.createElement('div');
        rem.className = 'doc-remarks';
        rem.innerHTML = '<span class="doc-key">remarks</span>' + escapeHtml(m.remarks || '(无注释)');
        doc.innerHTML = '';
        doc.appendChild(sig);
        doc.appendChild(sum);
        doc.appendChild(rem);
    }

    // ---------- dataset 预览 ----------
    async function previewDataset(dir) {
        const box = document.getElementById('datasetPreview');
        if (!box) return;
        box.innerHTML = '<div class="doc-empty"><span class="spinner"></span> 加载预览…</div>';
        try {
            const res = await fetch('/api/dataset/preview?dir=' + encodeURIComponent(dir));
            const data = await res.json();
            if (data.kind === 'none') {
                box.innerHTML = '<div class="doc-empty">该目录不含 dataset.ini / dataset.json</div>';
                return;
            }
            if (data.kind === 'ini') {
                const ini = data.ini || {};
                let html = '<div class="doc-sig">dataset.ini · ' + escapeHtml(ini.description || '无描述') + '</div>';
                html += '<div class="doc-key">后缀 ' + escapeHtml(ini.ext || '') + ' 匹配 ' + (ini.files ? ini.files.length : 0) + ' 个输入文件</div>';
                html += '<div class="dataset-files">';
                const files = (ini.files || []);
                // 惰性渲染：仅首屏展示前 50 个，滚动加载剩余
                const step = 50;
                let shown = 0;
                const renderMore = () => {
                    const frag = document.createDocumentFragment();
                    for (let i = shown; i < Math.min(shown + step, files.length); i++) {
                        const f = document.createElement('div');
                        f.className = 'dataset-file';
                        f.textContent = files[i];
                        frag.appendChild(f);
                    }
                    shown = Math.min(shown + step, files.length);
                    box.querySelector('.dataset-files').appendChild(frag);
                    if (shown < files.length && !box.querySelector('.dataset-more')) {
                        const more = document.createElement('button');
                        more.className = 'dataset-more';
                        more.textContent = '加载更多…';
                        more.addEventListener('click', renderMore);
                        box.querySelector('.dataset-files').after(more);
                    } else if (shown >= files.length && box.querySelector('.dataset-more')) {
                        box.querySelector('.dataset-more').remove();
                    }
                };
                html += '</div>';
                box.innerHTML = html;
                renderMore();
                return;
            }
            if (data.kind === 'json') {
                const j = data.json || {};
                let html = '<div class="doc-sig">dataset.json · ' + escapeHtml(j.description || '无描述') + '</div>';
                html += '<div class="doc-key">数据文件 ' + escapeHtml(j.datafile || '') + '</div>';
                html += '<div class="dataset-chunks"><table class="chunk-table"><thead><tr><th>#</th><th>offset</th><th>size</th></tr></thead><tbody>';
                const chunks = (j.chunks || []);
                const step = 50;
                let shown = 0;
                const tbody = () => box.querySelector('.chunk-table tbody');
                const afterHtml = '</tbody></table></div>';
                box.innerHTML = html;
                const renderMore = () => {
                    const frag = document.createDocumentFragment();
                    for (let i = shown; i < Math.min(shown + step, chunks.length); i++) {
                        const tr = document.createElement('tr');
                        tr.innerHTML = '<td>' + (i + 1) + '</td><td>' + chunks[i].offset + '</td><td>' + chunks[i].size + '</td>';
                        frag.appendChild(tr);
                    }
                    shown = Math.min(shown + step, chunks.length);
                    tbody().appendChild(frag);
                    if (shown < chunks.length && !box.querySelector('.dataset-more')) {
                        const more = document.createElement('button');
                        more.className = 'dataset-more';
                        more.textContent = '加载更多…';
                        more.addEventListener('click', renderMore);
                        box.querySelector('.dataset-chunks').after(more);
                    } else if (shown >= chunks.length && box.querySelector('.dataset-more')) {
                        box.querySelector('.dataset-more').remove();
                    }
                };
                box.insertAdjacentHTML('beforeend', afterHtml);
                renderMore();
                return;
            }
            box.innerHTML = '<div class="doc-empty">未知数据源类型</div>';
        } catch (e) {
            box.innerHTML = '<div class="doc-empty">预览异常：' + e.message + '</div>';
        }
    }

    function escapeHtml(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    // 打开弹窗时初始化两棵树
    if (btnOpen) btnOpen.addEventListener('click', () => {
        initTree('dllTree', 'dll');
        initTree('dataTree', 'data');
    });

    const submitBtn = document.getElementById('btnSubmit');
    if (submitBtn) {
        submitBtn.addEventListener('click', async () => {
            const assembly = document.getElementById('inAssembly').value.trim();
            const method = document.getElementById('inMethod').value.trim();
            const nameInput = document.getElementById('inName').value.trim();
            const datasetDir = document.getElementById('inDatasetDir').value.trim();
            const out = document.getElementById('submitResult');

            if (!assembly || !method) {
                out.textContent = '请选择 CLR Assembly 与目标方法。';
                return;
            }
            // 未设置任务名称时，按方法名自动生成（取 Class.Method）
            const autoName = nameInput || method.split('.').slice(-2).join('.');

            const params = new URLSearchParams({
                assemblypath: assembly,
                methodname: method,
                name: autoName,
                datasettype: datasetDir ? 'auto' : 'none'
            });
            if (datasetDir) params.set('datasetdir', datasetDir);

            out.textContent = '提交中…';
            try {
                const res = await fetch('/api/submit?' + params.toString());
                const data = await res.json();
                if (data.ok) {
                    out.textContent = '任务已提交，jobId: ' + data.jobId;
                    showToast('任务已提交：' + autoName, 'success', 3000);
                    currentTaskName = autoName;
                    try { localStorage.setItem('current-task-name', autoName); } catch (e) { /* 忽略 */ }
                    renderTaskChip();
                    setTimeout(closeSubmitModal, 800);
                } else {
                    out.textContent = '提交失败: ' + (data.message || '未知错误');
                    showToast('提交失败：' + (data.message || '未知错误'), 'error', 4000);
                }
            } catch (e) {
                out.textContent = '提交异常: ' + e.message;
                showToast('提交异常：' + e.message, 'error', 4000);
            }
        });
    }

    /* ============ 节点监控：视图模式切换 ============ */
    (() => {
        const seg = document.getElementById('monitorMode');
        if (!seg) return;
        seg.addEventListener('click', (e) => {
            const b = e.target.closest('.seg-btn');
            if (!b) return;
            const mode = b.dataset.mode;
            if (mode === monitorMode) return;
            monitorMode = mode;
            seg.querySelectorAll('.seg-btn').forEach((x) => x.classList.toggle('active', x === b));
            // 切换后立刻刷新显示
            if (lastStatus) {
                const isHeat = monitorMode === 'heatmap';
                document.getElementById('nodeList').hidden = isHeat;
                document.getElementById('heatLayout').hidden = !isHeat;
                if (isHeat) renderHeatmap(lastStatus);
                else document.getElementById('nodeDetail').hidden = true;
            }
        });
    })();

    /* ============ 算力热图：指标切换（已用 / 剩余）+ 点击方格查看节点详情 ============ */
    (() => {
        const toggle = document.getElementById('heatMetricToggle');
        if (toggle) {
            toggle.addEventListener('click', (e) => {
                const b = e.target.closest('.heat-metric-btn');
                if (!b) return;
                heatMetric = b.dataset.metric;
                toggle.querySelectorAll('.heat-metric-btn').forEach((x) => x.classList.toggle('active', x === b));
                if (monitorMode === 'heatmap' && lastStatus) renderHeatmap(lastStatus);
            });
        }

        const grid = document.getElementById('heatPower');
        if (grid) {
            grid.addEventListener('click', (e) => {
                const cell = e.target.closest('.heat-cell');
                if (!cell || !cell.dataset.nodeid) return;
                selectedNodeId = cell.dataset.nodeid;
                const n = (lastStatus ? lastStatus.nodes : []).find((x) => (x.nodeId || x.machineName) === selectedNodeId);
                renderNodeDetail(n || null);
                if (monitorMode === 'heatmap' && lastStatus) renderHeatmap(lastStatus); // 重绘高亮选中项
            });
        }

        const closeBtn = document.getElementById('nodeDetailClose');
        if (closeBtn) closeBtn.addEventListener('click', () => {
            selectedNodeId = null;
            const panel = document.getElementById('nodeDetail');
            if (panel) panel.hidden = true;
            if (monitorMode === 'heatmap' && lastStatus) renderHeatmap(lastStatus);
        });
    })();

    /* ============ echarts 历史曲线初始化 + 窗口自适应 ============ */
    (() => {
        try {
            if (typeof echarts === 'undefined') {
                console.warn('echarts 未加载，历史曲线不可用');
                return;
            }
            ensureHistoryChart();
            if (historyChart) {
                window.addEventListener('resize', () => historyChart.resize());
                // 首次渲染空图，待数据到来后由 tick 填充
                renderHistoryChart();
            }
        } catch (e) {
            console.warn('echarts 初始化失败：', e.message);
        }
    })();

    initThemeToggle();
    renderTaskChip();          // 初始化顶栏任务状态（恢复 localStorage 中的当前任务名）
    setClusterOffline(false);  // 初始为在线状态
    tick();
    setInterval(tick, POLL_MS);
})();
