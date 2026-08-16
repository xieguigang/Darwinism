// 仪表盘前端：每 1.5 秒轮询 /api/status 并局部刷新。
(() => {
    const POLL_MS = 1500;
    const $ = (id) => document.getElementById(id);

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
        }
    }

    tick();
    setInterval(tick, POLL_MS);
})();
