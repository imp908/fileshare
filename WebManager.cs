
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IWebManagers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebManagers
{

    /// -->In new overwritten
    ///Base web manager for sending request with type method and reading response to URL
    ///WebRequest, Httpwebresponse
    public class WebManager : IWebManager
    {

        public WebRequest _request;
        internal string OSESSIONID;

        public WebManager()
        {
            _request = null;
            this.OSESSIONID = string.Empty;
        }
        public WebRequest addRequest(string url, string method)
        {
            try
            {
                _request = WebRequest.Create(url);
                _request.Method = method;
                _request.ContentLength = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
            return _request;
        }
        internal void addHeader(HttpRequestHeader header, string value)
        {
            _request.Headers.Add(header, value);
        }
        internal string getHeaderValue(string header)
        {
            string result = string.Empty;
            result = this._request.GetResponse().Headers.Get(header);
            return result;
        }
        internal void addCredentials(NetworkCredential credentials)
        {
            this._request.Credentials = credentials;
        }
        public virtual WebResponse GetResponse(string url, string method)
        {

            addRequest(url, method);

            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public virtual async Task<HttpWebResponse> GetResponseAsync(string url, string method)
        {
            HttpWebResponse resp;
            addRequest(url, method);
            try
            {
                resp = (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }

            return await Task.FromResult(resp);
        }

    }

    /// <summary>
    /// DP Scope (data processing)    
    /// converts Responses to string
    /// </summary>
    public class WebResponseReader : IResponseReader
    {
        public string ReadResponse(WebResponse response)
        {
            string result = string.Empty;
            Stream sm = response.GetResponseStream();
            StreamReader sr = new StreamReader(sm);
            result = sr.ReadToEnd();
            return result;
        }
        public string ReadResponse(HttpWebResponse response)
        {
            string result = string.Empty;
            Stream sm = response.GetResponseStream();
            StreamReader sr = new StreamReader(sm);
            result = sr.ReadToEnd();
            return result;
        }
        public string ReadResponse(HttpResponseMessage response)
        {
            string result = string.Empty;
            System.Net.Http.HttpContent sm = response.Content;
            Task<Stream> sr = sm.ReadAsStreamAsync();
            Task<string> res = sm.ReadAsStringAsync();
            result = res.Result;
            return result;
        }
        public string ReadResponse(Task<HttpResponseMessage> response)
        {
            string result = string.Empty;
            Task<string> st = null;
            try
            {
                st = response.Result.Content.ReadAsStringAsync();
                result = st.Result;
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return result;
        }
    }

    /// <summary>
    /// WEB scope deprecation posible (Only if several credentials to different hosts needed, several DBs? )
    /// Contains Credentials for URI
    /// Currently unused
    /// </summary>    
    public class CredentialPool
    {
        CredentialCache credentialsCache;

        public void Add(Uri uri, string type, string username, string password)
        {
            credentialsCache = new CredentialCache();
            credentialsCache.Add(uri, type, new NetworkCredential(username, password));
        }
        public NetworkCredential credentials(Uri uri_, string type_)
        {
            return credentialsCache.GetCredential(uri_, type_);
        }
    }

}