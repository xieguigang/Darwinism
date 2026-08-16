// 仪表盘前端：每 1.5 秒轮询 /api/status 并局部刷新。
(() => {
    const POLL_MS = 1500;
    const $ = (id) => document.getElementById(id);

    // 节点监控视图模式：'full' 完整视图 / 'heatmap' 热图模式。
    let monitorMode = 'full';
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
                if (monitorMode === 'heatmap') {
                    const el = document.getElementById('heatmapPanel');
                    if (el && !el.hidden) renderHeatmap(lastStatus);
                }
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

    // 渲染热图模式：CPU 利用率热图 + 内存利用率热图。
    function renderHeatmap(s) {
        const nodes = (s.nodes || []).filter((n) => n.online);
        const cpuBox = $('heatCpu');
        const memBox = $('heatMem');

        const cell = (n, val) => {
            const name = escapeHtml(n.machineName || n.nodeId || '—');
            return `<div class="heat-cell" style="background:${heatColor(val)}">
                        <span class="hc-name">${name}</span>
                        <span class="hc-val">${val.toFixed(0)}%</span>
                        <span class="hc-state">${n.cores || 0} 核 · ${fmtMemMB(n.totalMemoryMB)}</span>
                    </div>`;
        };

        if (nodes.length === 0) {
            cpuBox.innerHTML = '<div class="empty">暂无在线节点</div>';
            memBox.innerHTML = '<div class="empty">暂无在线节点</div>';
            return;
        }
        cpuBox.innerHTML = nodes.map((n) =>
            cell(n, Math.max(0, Math.min(100, Number(n.cpuUsage) || 0)))).join('');
        memBox.innerHTML = nodes.map((n) =>
            cell(n, Math.max(0, Math.min(100, Number(n.memoryUsage) || 0)))).join('');
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
            renderOverview(s);
            renderNodes(s);
            renderFailures(s);
            renderLogs(s);
            renderMeta(s);

            // 视图模式切换显示
            const isHeat = monitorMode === 'heatmap';
            $('nodeList').hidden = isHeat;
            $('heatmapPanel').hidden = !isHeat;
            if (isHeat) renderHeatmap(s);

            // 历史采样 + 曲线（仅当前已渲染图表时更新）
            sampleHistory(s);
            if (historyInited) {
                ensureHistoryChart();
                renderHistoryChart();
            }
        } catch (e) {
            console.warn('状态轮询失败：', e.message);
            // 避免轮询异常刷屏：仅在当前无错误提示时轻提示一次
            showToast('状态轮询失败：' + e.message, 'error', 3000);
        }
    }

    // ============ 任务提交 ============
    const btn = document.getElementById('btnSubmit');
    if (btn) {
        btn.addEventListener('click', async () => {
            const assembly = document.getElementById('inAssembly').value.trim();
            const method = document.getElementById('inMethod').value.trim();
            const name = document.getElementById('inName').value.trim();
            const inputs = document.getElementById('inInputs').value.trim();
            const out = document.getElementById('submitResult');

            if (!assembly || !method) {
                out.textContent = '请填写 Assembly 路径与方法名。';
                return;
            }

            const params = new URLSearchParams({
                assemblypath: assembly,
                methodname: method
            });
            if (name) params.set('name', name);
            if (inputs) params.set('inputs', inputs);

            out.textContent = '提交中…';
            try {
                const res = await fetch('/api/submit?' + params.toString());
                const data = await res.json();
                if (data.ok) {
                    out.textContent = '任务已提交，jobId: ' + data.jobId;
                    showToast('任务已提交：' + (data.jobId || ''), 'success', 3000);
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

    /* ============ 提交任务面板：折叠 / 展开 ============ */
    (() => {
        const btn = document.getElementById('taskToggle');
        const body = document.getElementById('submitBody');
        if (!btn || !body) return;
        // 读取记忆的折叠状态
        let collapsed = false;
        try { collapsed = localStorage.getItem('task-panel-collapsed') === '1'; } catch (e) { /* 忽略 */ }
        const apply = () => {
            body.classList.toggle('collapsed', collapsed);
            btn.classList.toggle('collapsed', collapsed);
            btn.setAttribute('aria-expanded', String(!collapsed));
        };
        // 初始 max-height 以支持过渡动画
        body.style.maxHeight = collapsed ? '0px' : body.scrollHeight + 'px';
        apply();
        btn.addEventListener('click', () => {
            collapsed = !collapsed;
            body.style.maxHeight = collapsed ? '0px' : body.scrollHeight + 'px';
            apply();
            try { localStorage.setItem('task-panel-collapsed', collapsed ? '1' : '0'); } catch (e) { /* 忽略 */ }
        });
        window.addEventListener('resize', () => {
            if (!collapsed) body.style.maxHeight = body.scrollHeight + 'px';
        });
    })();

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
                document.getElementById('heatmapPanel').hidden = !isHeat;
                if (isHeat) renderHeatmap(lastStatus);
            }
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
    tick();
    setInterval(tick, POLL_MS);
})();
