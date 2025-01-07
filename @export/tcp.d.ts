// export R# package module type define for javascript/typescript language
//
//    imports "tcp" from "Darwinism";
//
// ref=Darwinism.Tcp@Darwinism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace tcp {
   /**
   */
   function local_address(tcp: object): object;
   /**
    * get a list of tcp port in used
    * 
    * 
   */
   function port_in_used(): object;
}
