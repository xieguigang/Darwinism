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

