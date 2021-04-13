/// <reference path="../linq.d.ts" />
var nodeStatus;
(function (nodeStatus) {
    nodeStatus[nodeStatus["busy"] = 0] = "busy";
    nodeStatus[nodeStatus["offline"] = 1] = "offline";
    nodeStatus[nodeStatus["idle"] = 2] = "idle";
})(nodeStatus || (nodeStatus = {}));
var protocols;
(function (protocols) {
    protocols[protocols["return_initialize_data"] = 0] = "return_initialize_data";
    protocols[protocols["log_events"] = 1] = "log_events";
})(protocols || (protocols = {}));
var app;
(function (app) {
    function start() {
        // 进行websocket的连接的建立
        // 并添加事件处理函数
        var ws = new WebSocket('ws://127.0.0.1:8000/');
        ws.onopen = function (evt) {
            console.log('Connection open ...');
            ws.send('get_initialize_data');
        };
        ws.onmessage = function (evt) {
            console.log('Received Message: ' + evt.data);
            processMessage(ws, evt);
        };
        ws.onclose = function (evt) {
            console.log('Connection closed.');
        };
    }
    app.start = start;
    function processMessage(ws, evt) {
        var msg = JSON.parse(evt.data);
        switch (msg.protocol) {
            case protocols.return_initialize_data:
                drawInterface(msg.msg);
                break;
            case protocols.log_events:
                logEvents(msg.msg);
            default:
                throw "not implements: " + msg.protocol;
        }
    }
    function logEvents(event) {
    }
    function drawInterface(data) {
        // 5个节点一行？
        var matrix = $ts("#grid");
        var columns = 5;
        var mat = $ts(data).Split(columns);
        for (var _i = 0, _a = mat.ToArray(false); _i < _a.length; _i++) {
            var mrow = _a[_i];
            var row = $ts("<tr>");
            // row
            matrix.appendChild(row);
        }
    }
})(app || (app = {}));
$ts(app.start);
//# sourceMappingURL=interface.js.map