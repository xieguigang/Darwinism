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
    * value between a given range?
    * 
    * 
     * @param name -
     * @param range -
     * @param env -
     * 
     * + default value Is ``null``.
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
    * create a index that used for the text similarity search
    * 
    * 
     * @param x -
     * @param fields -
   */
   function levenshtein_index(x: object, fields: string): object;
   /**
    * load in-memory table
    * 
    * 
     * @param x a dataframe object, a clr object array, or the file resource to a csv dataframe file.
     * @param nested_field use the nested property for make the clr object array index, this property name parameter value not working 
     *  for the dataframe or the table value, only works for the generic clr array object index.
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function load(x: any, nested_field?: string, env?: object): object;
   /**
    * create a full text search filter
    * 
    * 
     * @param name -
     * @param text -
     * @param boolean_mode set this parameter will use the levenshtein similarity matches method for index search
     * 
     * + default value Is ``true``.
   */
   function match_against(name: string, text: string, boolean_mode?: boolean): object;
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
