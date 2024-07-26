require(Darwinism);

let docker = Darwinism::container_id();

if (nchar(docker) == 0) {
    print("Not running inside a Docker container");
} else {
    print(`Running inside a docker container: ${docker}`);
}