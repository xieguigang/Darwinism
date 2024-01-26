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
}
