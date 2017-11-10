using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewsAPI.Models.Interfaces
{
    public interface IEntity
    {      
        [JsonProperty(Required = Required.Always)]
        string Content { get; set; }
        
        //[JsonProperty(Required = Required.Default)]
        //string other { get; set; }
        
        //[JsonProperty(Required = Required.Always)]
        //DateTime Created { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //DateTime Changed { get; set; }
    }
}
