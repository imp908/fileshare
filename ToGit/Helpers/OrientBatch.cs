using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAPI.Helpers
{
    public class OrientBatch
    {
        public OrientBatch(bool transaction, string type, string language, List<string> script)
        {
            Transaction = transaction;
            CommandList = new[] { new Command(type, language, script) };
        }

        [JsonProperty(PropertyName = "transaction")]
        public bool Transaction { get; set; }

        [JsonProperty(PropertyName = "operations")]
        public Command[] CommandList { get; set; }
    }

    public class Command
    {
        public Command(string type, string language, List<string> script)
        {
            Type = type;
            Language = language;
            Script = script;
        }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "script")]
        public List<string> Script { get; set; }
    }
}