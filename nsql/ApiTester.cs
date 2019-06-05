
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace APItesting
{

    /// <summary>
    /// Testing APIS
    /// Manages class for JSON API url,expected,ectual,ok values. Read/create + export JSON file with values.
    /// For every URL execute webrequest, reads string response, compares with Expected value, 
    /// changes statusess - OK(true/flase), Exception message - if needed. Exports result.
    /// </summary>
    public class APItester_sngltn
    {
      IWebManagers.IWebRequestManager webManager;
      IWebManagers.IResponseReader responseReader;

      private string Filename;
      public string URI {get; set;}
      //null - for explicit authentification turning off
      //""-empty string for basic NTLM authentification      
      public string BasicAuthCredentials {get; set;}
      //POST,GET methods
      public string Method {get; set;}
      //POST body text
      public string Body {get; set;}
      //exected value
      //null if null value is awaited
      //"not null" if not null
      //full value condition for one to one comarison
      public string Expected {get; set;}
      public string Actual {get; set;}
      public bool OK {get; set;}
      public string ExceptionText {get; set;}
      public string Comment {get; set;}

      public static List<APItester_sngltn> TestCases=new List<APItester_sngltn>();

      public void Initialize()
      {
        webManager=new WebManagers.WebRequestManager();
        responseReader=new WebManagers.WebResponseReader();

        Filename="test_urls.json";
        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + Filename))
        {
          Import();
        }
        else
        {
          APItester_sngltn.TestCases.Add(new APItester_sngltn()
          {
            URI="http://msk1-vm-ovisp01:8085/api/Person/GetManager/DegterevaSV"
            , Method="GET"
            , Expected="eliseevavv"
            ,OK=false
          });
          APItester_sngltn.TestCases.Add(new APItester_sngltn()
          {
            URI="http://msk1-vm-ovisp01:8085/api/Person/GetColleges/lobanovamg"
            ,Method="GET"
            ,Expected="[\"a.vagin\",\"Bagirovaev\",\"iku\",\"kotovaen\",\"stalmakovsm\"]"
            ,OK=false
          });
        }

        Export();

      }
      public void Export(string path=null)
      {
        string jStr=JsonConvert.SerializeObject(APItester_sngltn.TestCases, Formatting.Indented);
        string dir=AppDomain.CurrentDomain.BaseDirectory;
        if(path == null)
        {
          path=AppDomain.CurrentDomain.BaseDirectory;
        }
        File.WriteAllText(path + "\\" + Filename, jStr);
      }
      public void Import(string path=null)
      {
          if (path == null)
          {
              path=AppDomain.CurrentDomain.BaseDirectory;
          }
          string res=File.ReadAllText(path + "\\" + Filename);
          try{
          APItester_sngltn.TestCases=JsonConvert.DeserializeObject<List<APItester_sngltn>>(res);
          }catch(Exception e){ System.Diagnostics.Trace.WriteLine(e.Message); }
      }
      public void GO()
      {
        foreach (APItester_sngltn tc in APItester_sngltn.TestCases)
        {
          bool res=false;
          try
          {
            webManager.AddRequest(tc.URI);
            webManager.SetContent(tc.Body);
            if(tc.BasicAuthCredentials==string.Empty){
              webManager.NtlmAuth(true);
            }
            if(tc.BasicAuthCredentials==null){
              webManager.NtlmAuth(false);
            }
            if(!string.IsNullOrEmpty(tc.BasicAuthCredentials)){
              webManager.SetCredentials(new NetworkCredential(tc.BasicAuthCredentials.Split(':')[0]
              ,tc.BasicAuthCredentials.Split(':')[1]));
            }
            var resp=webManager.GetResponse(tc.Method);
            string rR=responseReader.ReadResponse(resp);

            if (tc.Expected.ToLower() == "not null")
            {
              if (rR != null)
              {
                res=true;
                tc.OK_(rR);
              }
                       
            }
            if (tc.Expected.ToLower() == "null")
            {
              if (rR == null)
              {
                res=true;
                tc.OK_(rR);
              }

            }
            if (tc.Expected == rR)
            {
              res=true;
              tc.OK_(rR);
            }
            if(res == false)
            {
              tc.NotOK_(rR);
            }

          }
          catch(WebException e)
          {
               
            //Handles error type names if they are expected
            //logs exception message
            if (tc.Expected == e.GetType().ToString())
            {
              tc.OK_(e.GetType().ToString(), new StreamReader(e.Response.GetResponseStream()).ReadToEnd());
            }
            else
            {
              tc.NotOK_(e.GetType().ToString(), new StreamReader(e.Response.GetResponseStream()).ReadToEnd());
            }
          }
          catch (Exception e)
          {
            //Handles error type names if they are expected
            //logs exception message
            if (tc.Expected == e.GetType().ToString())
            {
              tc.OK_(e.GetType().ToString(), e.Message);
            }
            else
            {
              tc.NotOK_(e.GetType().ToString(), e.Message);
            }
          }
        }

        Export();
      }

      public void OK_(string actual_)
      {
          this.Actual=actual_;
          this.ExceptionText=string.Empty;
          this.OK=true;
      }
      public void OK_(string actual_, string exception_)
      {
          this.Actual=actual_;
          this.ExceptionText=exception_;
          this.OK=true;
      }

      public void NotOK_(string actual_)
      {
          this.Actual=actual_;
          this.ExceptionText=string.Empty;
          this.OK=false;
      }
      public void NotOK_(string actual_, string exception_)
      {
        this.Actual=actual_;
        this.ExceptionText=exception_;
        this.OK=false;
       }

   }

    //class for collection of test cases with expected and result
    //currently unused due to refactor needed
    public class TestCase
    {
        public int CaseNumber {get; private set;}
        public string Input {get; set;}
        public string Expected {get; set;}
        public string Actual {get; private set;}
        public bool Equal {get; private set;}

        public TestCase()
        {
            this.CaseNumber += 1;
            this.Actual=string.Empty;
            this.Expected=string.Empty;
            this.Equal=false;
       }
        public void Check(string Actual_)
        {
            this.Actual=Actual_;
            if (this.Expected == this.Actual) {this.Equal=true;}
            else {this.Equal=false;}
       }
   }

}