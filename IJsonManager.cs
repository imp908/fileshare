using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IJsonManagers
{   
    public interface IJsonManger
    {

        IJEnumerable<JToken> ExtractFromParentNode(string input, string parentNodeName);
        IJEnumerable<JToken> ExtractFromParentChildNode(string input, string parentNodeName, string childNodeName);
        IJEnumerable<JToken> ExtractFromChildNode(string input, string childNodeName);

        string SerializeObject(object input_, JsonSerializerSettings settings_);
        string SerializeObject(object input_);

        IEnumerable<T> DeserializeFromParentNode<T>(string input, string parentNodeName) where T : class;
        IEnumerable<T> DeserializeFromParentChildNode<T>(string input, string parentNodeName, string childNodeName) where T : class;
        IEnumerable<T> DeserializeFromChildNode<T>(string input, string childNodeName) where T : class;

        IEnumerable<T> DeserializeFromParentNode<T>(string input) where T : class;
        IJEnumerable<JToken> ExtractFromParentNode(string input);

        IEnumerable<T> JTokensToCollection<T>(IEnumerable<JToken> input) where T : class;
        string CollectionToStringFormat<T>(IEnumerable<T> list_, JsonSerializerSettings jss = null) where T : class;

    }
}