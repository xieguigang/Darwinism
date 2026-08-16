// 仪表盘前端：每 1.5 秒轮询 /api/status 并局部刷新。
(() => {
    const POLL_MS = 1500;
    const $ = (id) => document.getElementById(id);

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
                try { localStorage.setItem(THEME_KEY, next); } catch (e) { /* 忽略 */ }
                showToast('已切换至 ' + (next === 'dark' ? '暗色' : '亮色') + '主题', 'info', 2000);
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

    function renderOverview(s) {
        $('clusterName').textContent = s.clusterName || 'Darwinism Cluster';
        $('onlineChip').textContent = '在线节点 ' + s.onlineNodes;
        $('mTotalNodes').textContent = (s.nodes || []).length;
        $('mOnline').textContent = s.onlineNodes;
        $('mCores').textContent = s.totalCores;
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

    function renderNodes(s) {
        const list = $('nodeList');
        const nodes = s.nodes || [];
        if (nodes.length === 0) {
            list.innerHTML = '<div class="empty">暂无节点上报</div>';
            return;
        }
        list.innerHTML = nodes.map((n) => `
            <div class="node-item">
                <div>
                    <div class="name">${escapeHtml(n.nodeId)}</div>
                    <div class="block">${n.currentBlock ? '块 ' + escapeHtml(n.currentBlock) : '空闲'}</div>
                </div>
                <div style="text-align:right">
                    <div class="block">${n.cores} 核 · ${fmtTime(n.lastHeartbeat)}</div>
                    <span class="status-dot ${n.online ? 'status-online' : 'status-offline'}"></span>
                    ${n.online ? '在线' : '失联'}
                </div>
            </div>`).join('');
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
            renderOverview(s);
            renderNodes(s);
            renderFailures(s);
            renderLogs(s);
            renderMeta(s);
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
                } else {
                    out.textContent = '提交失败: ' + (data.message || '未知错误');
                }
            } catch (e) {
                out.textContent = '提交异常: ' + e.message;
            }
        });
    }

    tick();
    setInterval(tick, POLL_MS);
})();
