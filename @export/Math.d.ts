// export R# package module type define for javascript/typescript language
//
//    imports "Math" from "Darwinism";
//
// ref=Darwinism.Math@Darwinism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * darwinism IPC parallel math
 * 
*/
declare namespace Math {
   /**
    * measure the average distance for the given dataset
    * 
    * 
     * @param x -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function average_distance(x: any, env?: object): number;
   /**
    * 
    * > this function only supports the cosine similarity score function.
    * 
     * @param x -
     * @param k -
     * 
     * + default value Is ``16``.
     * @param cutoff -
     * 
     * + default value Is ``0.8``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function knn(x: any, k?: object, cutoff?: number, env?: object): object;
   /**
     * @param prefilter_cor default value Is ``0.3``.
     * @param prefilter_pval default value Is ``0.05``.
     * @param n_trheads default value Is ``8``.
     * @param env default value Is ``null``.
   */
   function pearson_cor(x: any, y: any, prefilter_cor?: number, prefilter_pval?: number, n_trheads?: object, env?: object): object;
   /**
    * write the correlation network
    * 
    * 
     * @param cor -
     * @param file -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function write_network(cor: any, file: any, env?: object): any;
}
