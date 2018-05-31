using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace IJsonManagers
{  
  public interface IJsonManger
  {
    IJEnumerable<JToken> ExtractFromParentNode(string input);
    IJEnumerable<JToken> ExtractFromParentNode(string input, string parentNodeName);
    IJEnumerable<JToken> ExtractFromParentChildNode(string input, string parentNodeName, string childNodeName);
    IJEnumerable<JToken> ExtractFromParentChildren(string input, string childNodeName);


    string SerializeObject(object input_, JsonSerializerSettings settings_=null);
    string SerializeObject(object input_, JsonConverter converter_=null);
    string SerializeObject(object input_);


    IEnumerable<T> DeserializeFromParentNode<T>(string input, string parentNodeName) where T : class;
    IEnumerable<T> DeserializeFromParentNode<T>(string input) where T : class;
    IEnumerable<T> DeserializeFromParentChildNode<T>(string input, string parentNodeName, string childNodeName) where T : class;
    IEnumerable<T> DeserializeFromChildNode<T>(string input, string childNodeName) where T : class;
    IEnumerable<T> DeserializeFromParentChildren<T>(string input, string childNodeName) where T : class;
    IEnumerable<T> DeserializeFromParentNodeStringArr<T>(string input) where T : class;
    T DeserializeFromParentNodeStringObj<T>(string input) where T : class;

    IEnumerable<T> JTokensToCollection<T>(IEnumerable<JToken> input) where T : class;
    string CollectionToStringFormat<T>(IEnumerable<T> list_, JsonSerializerSettings jss=null) where T : class;

  }

}