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
   function __rscript_tmp(workdir: any): object;
   /**
   */
   function container_id(): object;
   /**
   */
   function get_mac_addresses(): object;
   /**
     * @param verbose default value Is ``true``.
   */
   function hardware_abstract(verbose?: any): object;
   /**
     * @param salt default value Is ````.
     * @param salt_bytes default value Is ``null``.
     * @param verbose default value Is ``false``.
   */
   function hardware_keys(salt?: any, salt_bytes?: any, verbose?: any): object;
   /**
     * @param salt default value Is ````.
     * @param salt_bytes default value Is ``null``.
     * @param verbose default value Is ``false``.
   */
   function hardware_sign(salt?: any, salt_bytes?: any, verbose?: any): object;
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
