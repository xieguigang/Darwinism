// export R# package module type define for javascript/typescript language
//
//    imports "memory_query" from "Darwinism";
//
// ref=Darwinism.MemoryQuery@Darwinism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * package tools for run in-memory query
 * 
*/
declare namespace memory_query {
   /**
    * set full text search index on data fields
    * 
    * 
     * @param x -
     * @param fields -
   */
   function fulltext(x: object, fields: string): object;
   /**
    * set hash term search index on data fields
    * 
    * 
     * @param x -
     * @param fields -
   */
   function hashindex(x: object, fields: string): object;
   /**
    * load in-memory table
    * 
    * 
     * @param x a dataframe object, or the file resource to a csv dataframe file
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function load(x: any, env?: object): object;
   /**
    * set value range search index on data fields
    * 
    * 
     * @param x -
     * @param fields a collection of the key-value tuples for create index fields,
     *  field = type, type should be one of the enum terms: number,integer,date
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function valueindex(x: object, fields: object, env?: object): object;
}
