using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAPI.Interfaces
{
    public interface IJSONProxy
    {
        IEnumerable<T> JSONfromCollectionNODE<T>(string input_) where T : class;
        IEnumerable<T> JSONfromCollectionNODE<T>(string input_, string name_) where T : class;

        IEnumerable<T> JSONfromNODEcollection<T>(string input_) where T : class;

        IEnumerable<T> DeserializeSample<T>(string resp) where T : class;
        IEnumerable<T> DeserializeFromNode<T>(string jInput, string Node) where T : class;
    }
}