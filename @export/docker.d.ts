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
     * @param workdir 
     * + default value Is ``null``.
     * @param mounts 
     * + default value Is ``null``.
     * @param portForward 
     * + default value Is ``null``.
   */
   function run(container: object, command: string, workdir?: string, mounts?: object, portForward?: object): string;
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
   */
   function stop(containers: string): ;
}
