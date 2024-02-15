// export R# package module type define for javascript/typescript language
//
//    imports "CDF" from "Darwinism";
//
// ref=Darwinism.CDF@Darwinism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace CDF {
   /**
    * open a connection to the netcdf file
    * 
    * 
     * @param file -
     * @return the file stream id
   */
   function open(file: string): object;
   /**
    * get variable data from given file
    * 
    * 
     * @param nc -
     * @param name -
   */
   function var_data(nc: object, name: string): any;
   /**
   */
   function vars(nc: object): string;
}
