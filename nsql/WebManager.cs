
using System;
using System.IO;
using System.Text;
using IWebManagers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Threading;
using System.Collections.Generic;
using System.Web;

using System.Security.Principal;

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
            _request=null;            
        }
        public WebManager(string login_,string password_)
        {
            if (login_ != null && password_ != null &&
                login_ != string.Empty && password_ != string.Empty)
            {
                NetworkCredential nc_ = new NetworkCredential(login_, password_);
                this._credentials = nc_;
            }                       
        }
        public WebRequest AddRequest(string url, string method)
        {
            try
            {
                _request=WebRequest.Create(url);
                _request.Method=method;
                _request.ContentLength=0;
                
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
            string result=string.Empty;
            result=this._request.GetResponse().Headers.Get(header);
            return result;
       }
        public void SetCredentials(NetworkCredential credentials)
        {
          _credentials=credentials;
          CredentialsBind();
        }
        public void SetCredentials(ICredentials cred_)
        {
          this._request.Credentials=cred_;          
        }
        public void CredentialsBind()
        {
            if (this._request != null)
            {
                if (this._credentials != null)
                {
                    this._request.Credentials=_credentials;
               }
           }
       }
        public void CredentialsUnBind()
        {
            if (this._request != null)
            {
                this._request.Credentials=null;
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
                string msg=new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
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
                resp=(HttpWebResponse)this._request.GetResponse();
           }
            catch (Exception e)
            {
                throw e;
           }

            return await Task.FromResult(resp);
       }

        public WebResponse GetResponse64(string method) {throw new NotImplementedException();}
        public WebResponse GetResponse(string method) {throw new NotImplementedException(); }
        public WebRequest AddRequest(string url) {throw new NotImplementedException();}

    }

    public class WebRequestManager : IWebRequestManager
    {

        public WebRequest _request;
        NetworkCredential _credentials;
        bool ntlmAuth = false;

        string GET="GET";
        string _url=null;
        object _lock=new object();
        int Timeout=0;
        byte[] _content=null;
        string _method=null;
        Dictionary<HttpRequestHeader, string> _headers 
           =new Dictionary<HttpRequestHeader, string>();
             
        public WebRequest AddRequest(string url)
        {

          if (url == null) {throw new Exception("String not passed");}
          SetUrl(url);
          this._request=WebRequest.Create(url);
          if(this._credentials!=null){ this._request.Credentials = this._credentials; }
          return this._request;            
            
        }
     
        public void SetUrl(string url_)
        {
          this._url=url_;
        }

        public void SetCredentials(NetworkCredential credentials)
        {        
          this._credentials=credentials;
        }
        public void NtlmAuth(bool swich_)
        {
          this.ntlmAuth = swich_;
        }
        internal void bindCredentials()
        {
          CheckReq();
          if (this._credentials != null)
          {
            this._request.Credentials=this._credentials;
          }            
        }
     
        public bool SetHeader(HttpRequestHeader header, string value)
        {
          if (_headers.ContainsKey(header)) {
            _headers.Remove(header);
          }
          _headers.Add(header, value);
          return true;
        }
        public bool RemoveHeader(HttpRequestHeader header)
        {
          if (_headers.ContainsKey(header))
          {
            _headers.Remove(header);
          }
          return true;
        }
        internal bool bindHeaders()
        {                                 
          foreach (KeyValuePair<HttpRequestHeader, string> pair in _headers)
          {
            this._request.Headers.Clear();
            if (pair.Value != null)
            {
              this._request.Headers.Add(pair.Key, pair.Value);
            }
          }
          return true;
        }
        public string GetHeader(string name)
        {
            string header=string.Empty;
            CheckReq();
            try
            {
                header=this._request.Headers.Get(name);
            } catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            return header;
        }

        public void SetContent(string value=null)
        {
            if (value == null)
            {
                _content=null;
            }
            else
            {
              _content = Encoding.UTF8.GetBytes(value);
            }
        }
        void bindContent()
        {         
          if (CheckContent() && this._request.Method != GET)
          {
            try
            {
              this._request.ContentType="application/json";
              this._request.ContentLength=_content.Length;
              string tempRes = _content.ToString();
                    
              using (Stream str=this._request.GetRequestStream())
              {
                //StreamWriter strw = new StreamWriter(str, new UTF8Encoding());
                //strw.Write(_content);
                str.Write(_content, 0, _content.Length);                       
              }
            }
            catch(Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}

          }
        }

        public void SetTimeout(int ms)
        {
          Timeout=ms;
        }
        void bindTimeout()
        {
          if (this._request != null && this.Timeout != 0)
          {
            this._request.Timeout=this.Timeout;
          }
        }

        public void SetBase64AuthHeader(string value)
        {
            SetHeader(HttpRequestHeader.Authorization, "Basic " + System.Convert.ToBase64String(
                Encoding.ASCII.GetBytes(value)
            ));
        }
        public void SetBase64AuthHeader()
        {
          if (this._credentials == null) {throw new Exception("Credentials not binded");}
          string value_=string.Format("{0}:{1}", this._credentials.UserName, this._credentials.Password);
          SetBase64AuthHeader(value_);
        }

        internal bool CheckReq()
        {
           
          if (_url == null) {throw new NoRequestBinded();}
             
          SwapRequestsURL(_url);
          return true;
        }
        internal bool CheckContent()
        {
          if (this._content == null) {return false;}
          if (this._content.Length == 0) {return false;}
          return true;
        }
        internal bool CheckURL(string url_=null)
        {
          if (url_ == null)
          {
            if (_url == null) {return false;}
            return true;
          }
          else
          {
            if (_url.Equals(url_))
            {
              return true;
            }
            else
            {
              return false;
            }
          }
        }

        public void SetMethod(string method_)
        {
          if (method_ != null)
          {
            _method=method_;                
            //if (method_ != GET){this._request.ContentLength=0;}
          }
          else {this._request.Method=GET;}
        }
        internal void bindMethod()
        {                   
          if (_method!=null)
          {              
            this._request.Method=_method;
          }
        }

        internal void SwapMethod(string method_)
        {
          SetMethod(method_);
          bindMethod();
          bindHeaders();
        }
        public void SwapRequestsURL(string url)
        {
          AddRequest(url);
          bindMethod();
          bindHeaders();
          //if (this._request.Method != GET)
          //{
          //    bindContent();
          //}                      
        }
        internal void SwapContent(string value_)
        {
            CheckReq();
            SetContent(value_);
            bindMethod();
            bindHeaders();
            if (this._request.Method != GET)
            {
                bindContent();
            }
        }

        public HttpWebResponse GetHttpResponse(string method_)
        {
            CheckReq();
            SwapMethod(method_);          
        
            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e) {throw e;}
        }

        public WebResponse GetResponse(string method_)
        {
          CheckReq();
          SwapMethod(method_);
          bindContent();
          bindTimeout();
          try
          {
              if(this.ntlmAuth){
                  this._request.Credentials = CredentialCache.DefaultCredentials;
                  WindowsIdentity wc=WindowsIdentity.GetCurrent();
              }
            return (HttpWebResponse)this._request.GetResponse();
          }
          catch(WebException e)
          {
            if(e.Response!=null){
            var val_=e.Response.Headers.GetValues("WWW-Authenticate");
              string resp=new StreamReader(e.Response.GetResponseStream()).ReadToEnd();                
            }
            return e.Response;              
          }
          catch (Exception e)
          {
            System.Diagnostics.Trace.WriteLine(e.Message);
            return null;
          }
        }
        public WebResponse GetResponse(string url_, string method)
        {
          CheckReq();
          SetUrl(url_);
          SetMethod(method);
          bindTimeout();
          try
          {
              if(this.ntlmAuth){ 
                  this._request.Credentials = CredentialCache.DefaultCredentials;
                  this._request.PreAuthenticate = true;
              }
            return (HttpWebResponse)this._request.GetResponse();
          }
          catch (Exception e)
          {
            throw e;
          }
        }

        public WebResponse GetResponse64(string method)
        {
           
            CheckReq();
            SetBase64AuthHeader();                    
            bindTimeout();                
            SwapMethod(method);
            bindContent();

            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch(WebException e)
            {
              if(e.Response!=null){
                string resp=new StreamReader(e.Response.GetResponseStream()).ReadToEnd();                
              }
              return e.Response;              
            }
            catch (Exception e){
                System.Diagnostics.Trace.WriteLine(e.Message);
              return null;
            }
        }
        public WebResponse GetResponse64(string url_, string method)
        {

            CheckReq();
            SetUrl(url_);
            SetBase64AuthHeader();
            SetMethod(method);
            bindTimeout();
            SwapMethod(method);

            try
            {
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
            string result=string.Empty;
            try
            {
                result=new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}
            return result;
        }
        public string ReadResponse(HttpWebResponse response)
        {
            string result=string.Empty;
			      try
            {
				        Stream sm=response.GetResponseStream();
				        StreamReader sr=new StreamReader(sm);
				        result=sr.ReadToEnd();
			      }
            catch(Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}
            return result;
        }
        public string ReadResponse(HttpResponseMessage response)
        {
            string result=string.Empty;
            System.Net.Http.HttpContent sm=response.Content;            
            Task<string> res=sm.ReadAsStringAsync();
            result=res.Result;
            return result;
        }
        public string ReadResponse(Task<HttpResponseMessage> response)
        {
            string result=string.Empty;
            Task<string> st=null;
            try
            {
                st=response.Result.Content.ReadAsStringAsync();
                result=st.Result;
            }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            return result;
        }
        public string ReadResponse(IHttpActionResult response)
        {
            string result  =null;
            Task<HttpResponseMessage> mes=response.ExecuteAsync(new System.Threading.CancellationToken());
            try
            {
                result=mes.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}
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
        this._returnedTask=ar_;
        this._result=result_;
      }

      async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
      {
        HttpResponseMessage response=new HttpResponseMessage(HttpStatusCode.OK);
        response.Content=new StringContent(_result, Encoding.UTF8, "text/plain");

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
            credentialsCache=new CredentialCache();
            credentialsCache.Add(uri, type, new NetworkCredential(username, password));
        }
        public NetworkCredential credentials(Uri uri_, string type_)
        {
            return credentialsCache.GetCredential(uri_, type_);
        }
    }

    public static class UserAuthenticationMultiple
    {

        public static string UserAcc(IPrincipal principal=null)
        {
            string result = String.Empty;
            bool ok = false;
            if (principal != null)
            {
              //net NTLM auth
              try
              {
                result = principal.Identity.Name.Split('\\')[1];
                ok = true;
              }
              catch (Exception e) { result += "Ipincipal no found;" + e.Message + "\r\n"; }
            }

            //Environment auth
            if (!ok)
            {
              try
              {
                result = HttpContext.Current.User.Identity.Name.ToString().Split('\\')[1];
                ok = true;
              }
              catch (Exception e) { result += "Current.User no found;"+ e.Message + "\r\n"; }
            }

            //context auth
            if (!ok)
            {
              try
              {
                result = Environment.UserName;
                ok = true;
              }
              catch (Exception e) { result += " Environment.UserName no found;"+ e.Message + "\r\n"; }
            }            
            
            return result;
        }
    }
    
}