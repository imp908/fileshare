using System.Net;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IWebManagers
{

    /// <summary>
    /// Gets response from URL with method
    /// </summary>
    public interface IWebManager
    {
        void addCredentials(NetworkCredential credentials);
        WebResponse GetResponse(string url, string method);
        WebRequest addRequest(string url, string method);
    }
    /// <summary>
    /// Reads response and converts it to string
    /// </summary>
    public interface IResponseReader
    {
        string ReadResponse(HttpWebResponse response);
        string ReadResponse(WebResponse response);
        string ReadResponse(HttpResponseMessage response);
    }

}