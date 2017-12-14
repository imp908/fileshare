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
    public interface IOrientEntity : IOrientObject
    {
        [JsonProperty("@rid", Order = 0)]
        string id { get; set; }
    }
    public interface IOrientVertex : IOrientEntity
    {
        [JsonProperty("@type")]
        string type {get; set;}       
        [JsonProperty("@version")]
        string version {get; set;}
        [JsonProperty("@class")]
        string class_ {get; set;}
    }
    //Specific for OrientDb (additional for Edges)
    public interface IOrientEdge : IOrientEntity
    {
        string Out {get; set;}
        string In {get; set;}
    }
    

}