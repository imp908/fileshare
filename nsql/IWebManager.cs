using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IWebManagers
{

      /// <summary>
    /// Gets response from URL with method
    /// </summary>
    public interface IWebManager
    {
        void SetCredentials(NetworkCredential credentials);
        WebResponse GetResponse(string method);
        WebResponse GetResponse64(string method);
        WebRequest AddRequest(string url);
        
    }

    public interface IWebRequestManager
    {
        WebRequest AddRequest(string url);
        string GetHeader(string name);
        HttpWebResponse GetHttpResponse(string method_);
        WebResponse GetResponse(string method);
        WebResponse GetResponse(string url_, string method);
        WebResponse GetResponse64(string method);
        WebResponse GetResponse64(string url_, string method);
        bool RemoveHeader(HttpRequestHeader header);
        void SetBase64AuthHeader();
        void SetBase64AuthHeader(string value);
        void SetContent(string value = null);
        void SetCredentials(NetworkCredential credentials);
        bool SetHeader(HttpRequestHeader header, string value);
        void SetMethod(string method_);
        void SetTimeout(int ms);
        void SetUrl(string url_);
        void SwapRequestsURL(string url);
    }

    /// <summary>
    /// Reads response and converts it to string
    /// </summary>
    public interface IResponseReader
    {
        string ReadResponse(HttpWebResponse response);
        string ReadResponse(WebResponse response);
        string ReadResponse(HttpResponseMessage response);
        string ReadResponse(IHttpActionResult response);
   }

}