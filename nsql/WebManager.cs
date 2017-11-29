
using System;
using System.IO;
using System.Text;
using IWebManagers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Threading;


namespace WebManagers
{

    public class NoRequestBinded : Exception
    {
        
    }

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
        public WebRequest AddRequest(string url, string method)
        {
            try
            {
                _request = WebRequest.Create(url);
                _request.Method = method;
                _request.ContentLength = 0;
                
                CredentialsBind();
                
            }
            catch (Exception e)
            {
                throw e;
            }
            return _request;
        }
        internal void AddHeader(HttpRequestHeader header, string value)
        {
            _request.Headers.Add(header, value);
        }
        public void AddBase64AuthHeader(string value)
        {
            _request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + System.Convert.ToBase64String(
            Encoding.ASCII.GetBytes(value)
            ));
        }
       
        internal string GetHeaderValue(string header)
        {
            string result = string.Empty;
            result = this._request.GetResponse().Headers.Get(header);
            return result;
        }
        public void AddCredentials(NetworkCredential credentials)
        {
            _credentials = credentials;
            CredentialsBind();
        }
        public void CredentialsBind()
        {
            if (this._request != null)
            {
                if (this._credentials != null)
                {
                    this._request.Credentials = _credentials;
                }
            }
        }
        public void CredentialsUnBind()
        {
            if (this._request != null)
            {
                this._request.Credentials = null;
            }
        }
        public virtual WebResponse GetResponseAuth(string url, string method)
        {

            AddRequest(url, method);            
            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public virtual WebResponse GetResponse(string url, string method)
        {

            AddRequest(url, method);
            CredentialsBind();
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
            CredentialsBind();
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
            AddRequest(url, method);
            CredentialsBind();
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

        public WebResponse GetResponse64(string method) { throw new NotImplementedException(); }
        public WebResponse GetResponse(string method) { throw new NotImplementedException();  }
        public WebRequest AddRequest(string url) { throw new NotImplementedException(); }

    }

    public class WebManager2 : IWebManager
    {

        WebRequest _request;
        NetworkCredential credentials;
        string GET="GET";
        string _url;
        object _lock = new object();

        public WebRequest AddRequest(string url)
        {
            if (_request == null)
            {
                _url = url;
                this._request = WebRequest.Create(url);
            }
            else
            {
                this._request = SwapRequests(url);
            }
            return this._request;
        }
        public HttpWebResponse GetHttpResponse(string method=null)
        {

            if (this._request == null)
            {
                throw new NoRequestBinded();
            }
           
            if (_url == null) { throw new NoRequestBinded(); }

            this._request = WebRequest.Create(_url);

                if (method != null)
                {
                    this._request.Method = method;
                    if (method != GET)
                    {
                        this._request.ContentLength = 0;
                    }

                }
            else { this._request.Method = GET; }
                try
                {
                    return (HttpWebResponse)this._request.GetResponse();

                }
                catch (Exception e) { throw e; }
            
          
        }

        public void AddCredentials(NetworkCredential credentials)
        {
            this.credentials=credentials;
        }
        public void BindCredentials()
        {
            if (this._request == null)
            {
                throw new NoRequestBinded();
            }
            if (this.credentials != null)
            {
                this._request.Credentials = this.credentials;
            }
            
        }

        internal WebRequest SwapRequests(string url)
        {
            WebRequest temp_request;
            temp_request = WebRequest.Create(url);

            temp_request.ContentType = this._request.ContentType;

            if (this._request.Method!=GET)
            {
                using(Stream strFrom = this._request.GetRequestStream())
                {
                    byte[] bt = new byte[strFrom.Length];
                    using(Stream strTo = temp_request.GetRequestStream())
                    {
                        strFrom.ReadAsync(bt, 0, bt.Length);
                        strTo.WriteAsync(bt, 0, bt.Length);
                    }               
                    
                }
                temp_request.ContentLength = this._request.ContentLength;
            }

            foreach(string header in temp_request.Headers)
            {
                if(header != "Host")
                {
                    temp_request.Headers = this._request.Headers;
                }
            }
        
            this._request = temp_request;
            return this._request;
        }
        public void Addheader(HttpRequestHeader header,string value)
        {
            if (this._request == null)
            {
                throw new NoRequestBinded();
            }
            this._request.Headers.Add(header, value);
        }
        public string GetHeader(string name)
        {
            string header=string.Empty;
            CheckReq();
            try
            {
                header = this._request.Headers.Get(name);
            } catch (Exception e) { }

            return header;
        }      
        public void AddContent(string value)
        {          
            CheckReq();
            if (this._request.Method != GET)
            {
                byte[] bt = null;
                try
                {
                    using (Stream str = this._request.GetRequestStream())
                    {
                        bt = new byte[value.Length];
                        str.WriteAsync(bt, 0, bt.Length);
                    }                  
                }
                catch (Exception e) { }
            }           
        }
        public void AddBase64AuthHeader(string value)
        {
            _request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + System.Convert.ToBase64String(
            Encoding.ASCII.GetBytes(value)
            ));
        }
        public void AddBase64AuthHeader()
        {         
                if (this.credentials == null)
                { throw new Exception("No credentials binded"); }

                _request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + System.Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", this.credentials.UserName, this.credentials.Password)
                    )
                ));            
                      
        }

        internal bool CheckReq()
        {
            if (this._request == null)
            {
                throw new NoRequestBinded();
            }
            else { return true; }
        }
        public void SetTimeout(int ms)
        {
            if(CheckReq())
            {
                this._request.Timeout = ms;
            }
        }
        public WebResponse GetResponse(string method)
        {
            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public WebResponse GetResponse64(string method)
        {
            if (this._request == null) { return null; }       

            try
            {
                AddBase64AuthHeader();
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }
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

    /// <summary>
    /// Wraps result in IhttpActionResult for ApiController return
    /// </summary>
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