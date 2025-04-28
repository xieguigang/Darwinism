
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

#' get platform hardware abstract report
#' 
const hardware_abstract = function() {
    let cpuinfo = readLines("/proc/cpuinfo") 
    |> which(s -> instr(s,"model name") > 0) 
    |> .Internal::tagvalue(":","")
    |> .Internal::trim()
    ;
    let threads = length(cpuinfo);
    let abstract = list(
        cpuinfo = cpuinfo,
        threads = threads,
        mac_list = get_mac_addresses()
    );

    print("get platform hardware abstract:");
    print("cpu info:");
    print(cpuinfo);
    print("abstract report:");
    str(abstract);

    return(abstract);
}

const get_mac_addresses = function() {
    let interfaces <- list.dirs("/sys/class/net", recursive = FALSE);
    interfaces <- basename(interfaces);
    interfaces <- interfaces[interfaces != "lo"];  # 排除回环接口
    interfaces <- as.list(interfaces, names = interfaces);

    let mac_list <- lapply(interfaces, function(intf) {
        let address_file <- file.path("/sys/class/net", intf, "address");

        if (file.exists(address_file)) {
            readLines(address_file, warn = FALSE);
        } else {
            "";
        }
    });

    return(mac_list);
}

const hardware_sign = function(salt = "", salt_bytes = NULL) {
    let abstract = hardware_abstract();

    require(JSON);

    abstract$salt_bytes <- md5(salt_bytes);
    abstract$salt <- salt;
    abstract <- JSON::json_encode(abstract);
    abstract <- md5(abstract);

    return(abstract);
}