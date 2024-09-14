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
     * @param env default value Is ``null``.
   */
   function between(name: string, range: any, env?: object): object;
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
     * @param x a dataframe object, a clr object array, or the file resource to a csv dataframe file.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function load(x: any, env?: object): object;
   /**
    * create a full text search filter
    * 
    * 
     * @param name -
     * @param text -
   */
   function match_against(name: string, text: string): object;
   /**
    * make dataframe query
    * 
    * 
     * @param x -
     * @param query -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function select(x: object, query: object, env?: object): any;
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
