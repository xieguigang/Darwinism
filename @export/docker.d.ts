// export R# package module type define for javascript/typescript language
//
//    imports "docker" from "Darwinism";
//
// ref=Darwinism.DockerTools@Darwinism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * Docker commands
 * 
*/
declare namespace docker {
   /**
     * @param img default value Is ``null``.
   */
   function docker(img?: string): object;
   /**
    * set environment variable for the docker run
    * 
    * 
     * @param docker -
     * @param args -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function env(docker: object, args: object, env?: object): any;
   /**
    * create docker image reference
    * 
    * 
     * @param name 
     * + default value Is ``null``.
     * @param publisher 
     * + default value Is ``null``.
     * @param env 
     * + default value Is ``null``.
   */
   function image(x: any, name?: string, publisher?: string, env?: object): object|object;
   /**
    * mount shared volumn between the host and the container
    * 
    * 
     * @param docker -
     * @param mount the folder path for shared, value of this parameter could be:
     *  
     *  1. just a single character vector for specific the path string
     *  2. a lambda expression for specific the different folder name
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function mount(docker: object, mount: any, env?: object): any;
   /**
    * List containers
    * 
    * 
   */
   function ps(): object;
   /**
    * delete docker images and related containers
    * 
    * 
   */
   function rmImage(imageId: string): boolean;
   /**
    * Run a command in a new container.(这个函数会捕捉到命令的标准输出然后以字符串的形式返回)
    * 
    * > 这个方法能够自定义的参数比较有限,如果需要更加复杂的使用方法,可以使用@``T:Darwinism.Docker.Environment``对象
    * 
     * @param command -
     * @param script 
     * + default value Is ``null``.
     * @param workdir 
     * + default value Is ``null``.
     * @param mounts 
     * + default value Is ``null``.
     * @param portForward 
     * + default value Is ``null``.
     * @param args 
     * + default value Is ``null``.
     * @param shell_cmdl this debug parameter specific that just returns the commandline for 
     *  run docker instead of run command and returns the std_output.
     * 
     * + default value Is ``false``.
     * @param env 
     * + default value Is ``null``.
   */
   function run(container: any, command: string, script?: string, workdir?: string, mounts?: object, portForward?: object, args?: object, shell_cmdl?: boolean, env?: object): string;
   /**
    * Search the Docker Hub for images
    * 
    * 
     * @param term -
   */
   function search(term: string): object;
   /**
    * Stop one or more running containers
    * 
    * 
     * @param containers -
     * 
     * + default value Is ``null``.
   */
   function stop_container(containers?: string): ;
   /**
    * enable interactive tty device
    * 
    * 
     * @param docker -
     * @param opt 
     * + default value Is ``true``.
   */
   function tty(docker: object, opt?: boolean): any;
   /**
    * set the workdir for run the command inside this new docker container
    * 
    * 
     * @param docker -
     * @param dir the directory path inside the docker container.
   */
   function workdir(docker: object, dir: string): any;
}
