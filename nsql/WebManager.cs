
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IWebManagers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Threading;


namespace WebManagers
{

    ///-->In new overwritten
    ///Base web manager for sending request with type method and reading response to URL
    ///WebRequest, Httpwebresponse
    public class WebManager : IWebManager
    {

        NetworkCredential _credentials;
        public WebRequest _request;
     
        public WebManager()
        {
            _request = null;            
        }
        public WebRequest addRequest(string url, string method)
        {
            try
            {
                _request = WebRequest.Create(url);
                _request.Method = method;
                _request.ContentLength = 0;
              
                bindCredentials();
                
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
        internal void add64Header(HttpRequestHeader header, string value)
        {
            _request.Headers.Add(header, "Basic " + System.Convert.ToBase64String(
            Encoding.ASCII.GetBytes(value)
            ));
        }
        internal string getHeaderValue(string header)
        {
            string result = string.Empty;
            result = this._request.GetResponse().Headers.Get(header);
            return result;
        }
        public void addCredentials(NetworkCredential credentials)
        {
            _credentials = credentials;
            bindCredentials();
        }
        public void bindCredentials()
        {
            if (this._request != null)
            {
                if (this._credentials != null)
                {
                    this._request.Credentials = _credentials;
                }
            }
        }
        public virtual WebResponse GetResponse(string url, string method)
        {

            addRequest(url, method);
            bindCredentials();
            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public virtual WebResponse GetResponse()
        {
            bindCredentials();
            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (System.Net.WebException e)
            {                           
                string msg = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                System.Diagnostics.Trace.WriteLine(e.Message);
                System.Diagnostics.Trace.WriteLine(msg);
                throw new Exception(msg);
            }           
           
        }
        public virtual async Task<HttpWebResponse> GetResponseAsync(string url, string method)
        {
            HttpWebResponse resp;
            addRequest(url, method);
            bindCredentials();
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
            try
            {
                result = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception e){ System.Diagnostics.Trace.WriteLine(e.Message); }
            return result;
        }
        public string ReadResponse(HttpWebResponse response)
        {
            string result = string.Empty;
			try
            {
				Stream sm = response.GetResponseStream();
				StreamReader sr = new StreamReader(sm);
				result = sr.ReadToEnd();
			}
            catch(Exception e){ System.Diagnostics.Trace.WriteLine(e.Message); }
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
        public string ReadResponse(IHttpActionResult response)
        {
            string result  =null;
            Task<HttpResponseMessage> mes = response.ExecuteAsync(new System.Threading.CancellationToken());
            try
            {
                result = mes.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return result;
        }

    }

    public class ReturnEntities : IHttpActionResult
    {
        HttpRequestMessage _returnedTask;
        public string _result;

        public ReturnEntities(string result_, HttpRequestMessage ar_)
        {
            this._returnedTask = ar_;
            this._result = result_;
        }

        async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(_result, Encoding.UTF8, "text/plain");

            return await Task.FromResult(response);
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