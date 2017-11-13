using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using IQueryManagers;

namespace IOrientObjects
{
    
	
    //For all OrientDatabaseToken objects (Vertexes and Edges both)
	    //For all OrientDatabaseToken objects (Vertexes and Edges both)   
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
    public interface IOrientVertex
    {
        [JsonProperty("@type")]
        string type { get; set; }
        [JsonProperty("@rid", Order = 0)]
        string id { get; set; }
        [JsonProperty("@version")]
        string version { get; set; }
        [JsonProperty("@class")]
        string class_ { get; set; }
    }
    //Specific for OrientDatabaseToken (additional for Edges)
    public interface IOrientEdge : IOrientVertex
    {
        string Out { get; set; }
        string In { get; set; }
    }

    public interface ITypeConverter
    {
        void Add(Type type_, ITypeToken token_);
        ITypeToken Get(IOrientVertex object_);
        ITypeToken GetBase(IOrientVertex object_);
        ITypeToken Get(Type type_);
        ITypeToken GetBase(Type type_);
    }

}