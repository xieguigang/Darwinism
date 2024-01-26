
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
    
}