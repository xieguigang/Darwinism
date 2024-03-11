
#' Configs of the default parallel runtime environment
#' 
const ___config_runtime_env = function() {
    Environment::set_threads(32);
    Environment::set_libpath(@HOME);
}

#' Check of netstat tool is installed or not
#' 
#' @details this function only works for linux platform
#' 
const ___check_netstat = function() {
    if ((Sys.info()[['sysname']]) != "Win32NT") {
        if (!centos::check_command_exists("netstat")) {
            no_netstat_warning();
        }
    }
}

#' show warning message about missing ``netstat`` command
#' 
#' @details create a slave task via ``IPCSocket`` needs check of the
#' assigned port number locally, for windows system, win32 api could 
#' be used for this job; for unix system, the ``netstat`` command will
#' be used for check this status data.
#' 
const no_netstat_warning = function() {
    let message = [
        "warning: no `netstat` command could be found on your linux system, ipc parallel may be run into a network error!",
        "please consider install the command at first and then run the computing task again: sudo yum install net-tools"
    ];

    print(message);
    warning(message);

    invisible(NULL);
}