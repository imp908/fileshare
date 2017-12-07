using Newtonsoft.Json;

namespace IOrientObjects
{

    //For all OrientDb objects (Vertexes and Edges both)   
    public interface IOrientObject
    {

   }
    public interface IOrientDatabase : IOrientObject
    {

   }
    public interface IOrientClass : IOrientObject
    {

   }
    public interface IOrientProperty : IOrientObject
    {

   }
    public interface IOrientVertex : IOrientObject
    {
        [JsonProperty("@type")]
        string type {get; set;}
        [JsonProperty("@rid", Order=0)]
        string id {get; set;}
        [JsonProperty("@version")]
        string version {get; set;}
        [JsonProperty("@class")]
        string class_ {get; set;}
   }
    //Specific for OrientDb (additional for Edges)
    public interface IOrientEdge : IOrientVertex
    {
        string Out {get; set;}
        string In {get; set;}
   }

   

}