using HtmlAgilityPack;
using NewsAPI.Extensions;
using NewsAPI.Interfaces;
using NewsAPI.Models.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NewsAPI.Implements
{
    public class OrientNewsJsonValidator : IJsonValidator
    {
        
        public bool Validate(string json)
        {
            var jsonSchemaGenerator = new JsonSchemaGenerator();
            var myType = typeof(IEntity);
            var schema = jsonSchemaGenerator.Generate(myType);
            JObject entity=null;

            try
            {
                entity = JObject.Parse(json);
           
                if (entity.IsValid(schema))
                {
                    var ok = ConfigurationManager.AppSettings["elements_included_html"];
                    string[] elementsIncludingHtml = ConfigurationManager.AppSettings["elements_included_html"].Split(',');
                    var htmlElements = entity.ToObject<Dictionary<string, string>>().Where(w => w.Key.ContainsAny(elementsIncludingHtml));

                    foreach (var htmlElement in htmlElements)
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(htmlElement.Value);
                        if (doc.ParseErrors.Count() == 0)
                        { continue; }
                        else
                        {
                            return false;
                            //throw new Exception(String.Format("html content of json string did not pass html validating. Element name: '{0}'.",htmlElement.Key));
                        }
                    }
                    return entity.IsValid(schema);

                }
                else
                {
                    return false;
                    //throw new Exception("json string did not pass validation by schema");
                }

            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                System.Diagnostics.Trace.Write(e.Message);
            }

            return false;
        }

    }
}