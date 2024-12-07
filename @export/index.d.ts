// export R# source type define for javascript/typescript language
//
// package_source=Darwinism

declare namespace Darwinism {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   /**
   */
   function ___check_netstat(): object;
   /**
   */
   function ___config_runtime_env(): object;
   /**
     * @param debug default value Is ``false``.
   */
   function __call_rscript_docker(image_id: any, script_code: any, workdir: any, mount: any, debug?: any): object;
   /**
   */
   function container_id(): object;
   /**
   */
   function no_netstat_warning(): object;
   /**
     * @param source default value Is ``null``.
     * @param debug default value Is ``false``.
     * @param workdir default value Is ``null``.
     * @param mount default value Is ``Call "list"()``.
   */
   function run_rlang_interop(code: any, image: any, source?: any, debug?: any, workdir?: any, mount?: any): object;
}
