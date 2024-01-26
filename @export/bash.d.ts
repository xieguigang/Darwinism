// export R# package module type define for javascript/typescript language
//
//    imports "bash" from "Darwinism";
//
// ref=Darwinism.Bash@Darwinism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * An automation pipeline toolkit build for cloud computing
 * 
 * > working on windows for connect remote linux server via putty
*/
declare namespace bash {
   /**
    * Execute a command on remote linux server session.
    * 
    * 
     * @param ssh remote linux ssh session
     * @param command -
     * @param arguments -
     * 
     * + default value Is ``null``.
   */
   function exec(ssh: object, command: string, arguments?: string): string;
   /**
    * Run a script on remote linux server session.
    * 
    * 
     * @param ssh remote linux ssh session
     * @param script -
   */
   function run(ssh: object, script: string): string;
   /**
    * Create a new remote linux ssh session
    * 
    * 
     * @param endpoint 
     * + default value Is ``'127.0.0.1'``.
     * @param port 
     * + default value Is ``22``.
     * @param plink -
     * 
     * + default value Is ``'plink'``.
     * @param debug 
     * + default value Is ``false``.
   */
   function ssh(user: string, password: string, endpoint?: string, port?: object, plink?: string, debug?: boolean): object;
}
