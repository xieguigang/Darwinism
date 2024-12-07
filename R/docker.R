#' check of the script is running inside a docker container or not?
#' 
#' @return this function returns a character vector of the current
#'    docker container id if script process is running inside a docker 
#'    container environment, or nothing if not.
#'  
const container_id = function() {
    if (file.exists("/.dockerenv") && file.exists("/proc/1/cgroup")) {
        let docker_cgroup = readLines("/proc/1/cgroup");
        let container = $"/docker/.+"(docker_cgroup);

        # [root@459b7205dc04 MSI]# cat /proc/1/cgroup
        # 11:perf_event:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 10:devices:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 9:memory:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 8:freezer:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 7:blkio:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 6:pids:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 5:hugetlb:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 4:cpuacct,cpu:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 3:net_prio,net_cls:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 2:cpuset:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        # 1:name=systemd:/docker/459b7205dc04f9b274996918ba5e333afbeb74e53b1cd8230277819f67c8c057
        container <- container |> unlist() |> strsplit("/", fixed = TRUE);
        container <- container@{3};

        # returns the container id
        unique(container);
    } else {
        NULL;
    }
}

#' Interop run rscript in a docker container
#' 
#' @param code a R# closure code for run in the target docker container
#' @param image the docker image id for run Rscript interop
#' @param mount the volumn for mount in the target docker container
#'  
const run_rlang_interop = function(code, image, source = NULL, debug = FALSE, 
        workdir = NULL, 
        mount = list()) {

    let script_code = transform_rlang_source(code, source, 
            debug = debug);

    image |> __call_rscript_docker(script_code, 
        workdir = workdir, 
        mount = mount, 
        debug = debug);
}

const __rscript_tmp = function(workdir) {
    let mount_tmp = readLines("/etc/mtab") 
    |> grep("/tmp", fixed = TRUE)
    ; 

    if (mount_tmp) {
        tempfile(fileext = ".R");
    } else {
        # write rscript to workspace
        mount_tmp <- basename(tempfile(fileext = ".R"));
        mount_tmp <- file.path(workdir, `.r_lang/${mount_tmp}.R`);
        mount_tmp;
    }
}

#' A helper function for run Rscript inside a docker container
#' 
#' @param script_code a text data of the script for run
#' @param image the docker image id for run in the docker.
#' 
const __call_rscript_docker = function(image_id, script_code, workdir, mount, 
                                            debug = FALSE) {
    imports "docker" from "Darwinism";

    let code_save  = __rscript_tmp(workdir);
    let current_wd = getwd();
    let change_wd  = nchar(workdir) > 0;
    
    if (change_wd) {
        setwd(workdir);
    }

    print("run script at workspace:");
    print(getwd());
    print(code_save);
    print(`Rscript "${code_save}"`);

    writeLines(script_code, con = code_save);
    # system(`Rscript "${code_save}"`);
    # run in docker container
    let cmdl_debug = docker 
    |> docker::image(image_id)
    |> docker::tty(opt = FALSE)
    # |> env(LD_LIBRARY_PATH = "/opt/R/4.0.3/lib/R/lib")
    # |> env(R_HOME = "/opt/R/4.0.3/lib/R")
    |> docker::mount(getwd())
    |> docker::mount("/var/run/docker.sock" -> "/var/run/docker.sock")
    |> docker::mount("/usr/bin/docker")
    |> docker::mount(mount)
    |> docker::mount(code_save)
    # |> docker::workspace(getwd())
    |> docker::run("Rscript", code_save, 
        workdir = getwd(), 
        shell_cmdl = debug)
    ;

    print(cmdl_debug);

    if (change_wd) {
        setwd(current_wd);
    }

    print("end of rlang_interop~~~");
}