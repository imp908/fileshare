using Newtonsoft.Json;
using System;

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
      
        [JsonProperty("@rid", Order = 1)]
        string id { get; set; }
        [JsonProperty("@type")]
        string type {get;}          
        [JsonProperty("@version")]
        string version {get; }   
        [JsonProperty("@class")]
        string class_ {get; set;} 
    }
    public interface IOrientVertex : IOrientEntity
    {    
      
    }
    //Specific for OrientDb (additional for Edges)
    public interface IOrientEdge : IOrientEntity
    {
        //string Out {get; set;}
        //string In {get; set;}
    }
    
    public interface IorientDefaultObject : IOrientEntity
    {
        [JsonProperty("GUID", Order = 2)]
        string GUID { get; set; }
        [JsonProperty("Created", Order = 3)]
        DateTime? created { get; set; } 
        [JsonProperty("Changed", Order = 4)]
        DateTime? changed { get; set; }
        [JsonProperty("Disabled", Order = 5)]
        DateTime? disabled { get; set; }
    }

}