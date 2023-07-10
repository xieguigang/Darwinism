/// <reference path="../linq.d.ts" />

interface GridNode {

    guid: string;
    status: nodeStatus;
    ip: string;
    cpu_load: number;
    memory_load: number;
    memory_installed: number;
    memory_used: number;

}

enum nodeStatus {
    busy, offline, idle
}

enum protocols {
    return_initialize_data,
    log_events
}

interface message {
    protocol: protocols;
    msg: GridNode[] | string;
}

module app {

    export function start() {
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

    function processMessage(ws: WebSocket, evt: MessageEvent) {
        let msg: message = JSON.parse(<string>evt.data);

        switch (msg.protocol) {
            case protocols.return_initialize_data:
                drawInterface(<GridNode[]>msg.msg);
                break;
            case protocols.log_events:
                logEvents(<string>msg.msg);
            default:
                throw `not implements: ${msg.protocol}`;
        }
    }

    function logEvents(event: string) {

    }

    function drawInterface(data: GridNode[]) {
        // 5个节点一行？
        let matrix: HTMLBodyElement = <any>$ts("#grid");
        let columns: number = 5;
        let mat: IEnumerator<GridNode[]> = $ts(data).Split(columns);

        for (let mrow of mat.ToArray(false)) {
            let row = $ts("<tr>");

            // row
            matrix.appendChild(row);
        }
    }
}

$ts(app.start);