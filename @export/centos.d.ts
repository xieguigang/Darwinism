// export R# package module type define for javascript/typescript language
//
//    imports "centos" from "Darwinism";
//
// ref=Darwinism.CentosTools@Darwinism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
 * > this module only works on linux system
*/
declare namespace centos {
   /**
    * check command is existsed on linux system or not?
    * 
    * 
     * @param command -
     * @param env 
     * + default value Is ``null``.
   */
   function check_command_exists(command: string, env?: object): boolean;
   /**
     * @param x default value Is ``'-tulnp'``.
     * @param verbose default value Is ``false``.
     * @param env default value Is ``null``.
   */
   function netstat(x?: string, verbose?: boolean, env?: object): any;
}
