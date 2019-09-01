/// <reference path="../../linq.d.ts" />
interface GridNode {
    guid: string;
    status: nodeStatus;
    ip: string;
    cpu_load: number;
    memory_load: number;
    memory_installed: number;
    memory_used: number;
}
declare enum nodeStatus {
    busy = 0,
    offline = 1,
    idle = 2
}
declare enum protocols {
    return_initialize_data = 0,
    log_events = 1
}
interface message {
    protocol: protocols;
    msg: GridNode[] | string;
}
declare module app {
    function start(): void;
}
