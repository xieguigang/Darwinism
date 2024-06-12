require(Darwinism);

imports "docker" from "Darwinism";

let wd = "/path/to/host/folder/"; 
let cmdl_debug = docker 
|> image("analysis_image:20240612_1")
|> tty()
|> env(LD_LIBRARY_PATH = "/opt/R/4.0.3/lib/R/lib")
|> env(R_HOME = "/opt/R/4.0.3/lib/R")
|> mount(wd)
|> mount("/var/run/docker.sock" -> "/run/docker.sock")
|> mount("/bin/docker")
|> mount("/mnt/datapool/")
|> workspace(wd)
|> run("python3", "/mnt/diff.py", shell_cmdl = TRUE, args = list(
    "-anno" = "a.csv",
    "-peak" = "a vs b",
    "-sample" = "/data/sampleinfo.csv",
    "-json" = "prepa.json",
    "-out" = "/mount/output_dir",
    "-m" = "PLSDA",
    "-log" = "log2"
))
;

print(cmdl_debug);