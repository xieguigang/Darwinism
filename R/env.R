
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

#' Get Platform Hardware Abstract Report
#' 
#' This function retrieves key hardware information from the system, including CPU details 
#' and MAC addresses of network interfaces. It provides both raw data and a structured summary.
#'
#' @param verbose Logical. If TRUE (default), prints detailed system information to console.
#'
#' @return A list containing three components:
#' \itemize{
#'   \item \strong{cpuinfo}: Character vector of CPU model names (one per thread)
#'   \item \strong{threads}: Integer count of logical processors/threads
#'   \item \strong{mac_list}: List of non-empty MAC addresses for network interfaces
#' }
#'
#' @examples
#' \dontrun{
#' hardware_info <- hardware_abstract()
#' str(hardware_info)
#' }
#' @note
#' This function requires access to system files:
#' - /proc/cpuinfo for processor information
#' - /sys/class/net for network interface data
#' May require elevated privileges on some systems.
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
        # get mac address list and
        # filter out the empty mac address
        mac_list = get_mac_addresses() |> which(mac -> nchar(mac) > 0)
    );

    if (verbose) {
        print("get platform hardware abstract:");
        print("cpu info:");
        print(cpuinfo);
        print("abstract report:");
        str(abstract);

        # [1] "get platform hardware abstract:"
        # [1] "cpu info:"
        # [1]     "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [4]     "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [7]     "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [10]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [13]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [16]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [19]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [22]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [25]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [28]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [31]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [34]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [37]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [40]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [43]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [46]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [49]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [52]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [55]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [58]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [61]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [64]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [67]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [70]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [73]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [76]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [79]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [82]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [85]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [88]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [91]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [94]    "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Processor" 
        # [1] "abstract report:"
        # List of 3
        #  $ cpuinfo  : chr [1:96] "AMD EPYC 7402 24-Core Processor" "AMD EPYC 7402 24-Core Process"| __truncated__ ...
        #  $ threads  : int 96
        #  $ mac_list : List of 1
        #  ..$ eth0 : chr "76:97:d6:65:12:63"
    }

    return(abstract);
}

#' Retrieve MAC Addresses of Network Interfaces
#' 
#' Scans system network interfaces and extracts MAC addresses, excluding loopback interfaces.
#'
#' @return A named list where:
#' \itemize{
#'   \item Names are network interface names (e.g., "eth0")
#'   \item Values are corresponding MAC addresses (empty string if unavailable)
#' }
#'
#' @examples
#' \dontrun{
#' macs <- get_mac_addresses()
#' }
#' @note
#' Requires access to /sys/class/net directory structure.
#' Specifically ignores the loopback interface (lo).
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

#' Generate Hardware-based Digital Signatures
#' 
#' Creates unique MD5 checksums based on system hardware characteristics and optional salt.
#' Signatures are MAC-address specific, generating one checksum per network interface.
#'
#' @param salt Character string (typically user email) to add entropy to signatures.
#' @param salt_bytes Optional raw bytes as additional salt. Will be MD5-hashed if provided.
#' @param verbose Logical. Show debug information during generation (default: FALSE).
#'
#' @return Character vector of MD5 checksums:
#' \itemize{
#'   \item Length matches number of valid network interfaces
#'   \item Order corresponds to interfaces returned by get_mac_addresses()
#'   \item Each checksum incorporates hardware profile + MAC address + salt(s)
#' }
#'
#' @examples
#' \dontrun{
#' signatures <- hardware_sign(salt = "user@example.com")
#' }
#'
#' @seealso
#' \code{\link{hardware_abstract}} for underlying hardware data collection
#' @note
#' Requires the JSON package for data serialization. Signature components include:
#' - CPU model list - Number of threads
#' - Salt values (both string and hashed bytes)
#' - Per-interface MAC address
const hardware_sign = function(salt = "", salt_bytes = NULL, verbose = FALSE) {
    let abstract = hardware_abstract(verbose = verbose);
    let mac_list = abstract$mac_list;

    require(JSON,quietly=TRUE);

    abstract$mac_list <- NULL;
    abstract$salt_bytes <- md5(as.raw(salt_bytes));
    abstract$salt <- salt;

    let sign = sapply(mac_list, function(mac) {
        let data = as.list(abstract);
        data$mac = mac;
        data = JSON::json_encode(data);
        return(md5(data));
    });

    return(sign);
}

#' get and print the generated hardware keys
#' 
const hardware_keys = function(salt = "", salt_bytes = NULL, verbose = FALSE) {
    print(hardware_sign(salt, salt_bytes, verbose));
}