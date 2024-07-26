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
        container <- container@{2};

        # returns the container id
        unique(container);
    } else {
        NULL;
    }
}