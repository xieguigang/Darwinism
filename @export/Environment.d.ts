// export R# package module type define for javascript/typescript language
//
//    imports "Environment" from "Darwinism";
//
// ref=Darwinism.Env@Darwinism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * the IPC parallel environment
 * 
*/
declare namespace Environment {
   /**
   */
   function set_libpath(libpath: string): object;
   /**
    * set the parallel batch threads
    * 
    * 
     * @param n_threads -
   */
   function set_threads(n_threads: object): object;
}
