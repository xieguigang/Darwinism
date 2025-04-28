
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
const hardware_abstract = function(verbose = TRUE) {
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

    if (verbose) {
        print("get platform hardware abstract:");
        print("cpu info:");
        print(cpuinfo);
        print("abstract report:");
        str(abstract);
    }

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

#' make digital signs for current platform hardware.
#' 
#' @param salt the salt string for make the digital sign, usually be the account email address.
#' @param salt_bytes a vector of the raw bytes binary data for this digital sign.
#' @param verbose show verbose debug echo
#' 
#' @return a character vector of the current hardware digital sign checksum.
#'   the length of this checksum vector is equals to the size of the network
#'   driver. which means make sign for each corresponding network driver based
#'   on its mac address.
#' 
const hardware_sign = function(salt = "", salt_bytes = NULL, verbose = FALSE) {
    let abstract = hardware_abstract(verbose = verbose);
    let mac_list = abstract$mac_list;

    require(JSON,quietly=TRUE);

    abstract$mac_list <- NULL;
    abstract$salt_bytes <- md5(salt_bytes);
    abstract$salt <- salt;

    let sign = sapply(mac_list, function(mac) {
        let data = as.list(abstract);
        data$mac = mac;
        data = JSON::json_encode(data);
        return(md5(data));
    });

    return(sign);
}