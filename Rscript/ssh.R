imports "bash" from "Centos";

let linux = ssh("xcms", "12345", "192.168.1.253");

linux :> exec("ls", '-l \"/mnt/smb/tmp\"') :> print;
linux :> exec("Rscript", "-e \"biodeep::run.Deconvolution(raw = '/mnt/smb/tmp/test_watcher/p+n/neg/mzML', n_threads = 4);\"");