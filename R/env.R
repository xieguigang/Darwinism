
#' Configs of the default parallel runtime environment
#' 
const ___config_runtime_env = function() {
    Environment::set_threads(32);
    Environment::set_libpath(@HOME);
}