
using System;
using System.Collections.Generic;

using System.Net;

using System.Configuration;


using IQueryManagers;
using QueryManagers;

using IOrientObjects;
using IJsonManagers;
using IWebManagers;
using IRepos;
using OrientRealization;

namespace Repos
{

    public class Repo : IRepo
    {

        IJsonManger jm;
        ITokenBuilder tb;
        ITypeConverter tk;
        ICommandBuilder cb;
        IWebManager wm;
        IResponseReader ir;

        OrientWebManager owm=new OrientWebManager();

        string AuthUrl, CommandUrl, QueryUrl, DatabaseUrl;

        public Repo(
            IJsonManger jsonManager_, ITokenBuilder tokenBuilder_, ITypeConverter typeConverter_, ICommandBuilder commandBuilder_,
            IWebManager webManger_, IResponseReader responseReader_)
        {

            this.jm=jsonManager_;
            this.tb=tokenBuilder_;
            this.tk=typeConverter_;
            this.cb=commandBuilder_;
            this.wm=webManger_;
            this.ir=responseReader_;

            cb.BindTokens(TokenRepo.authUrl);cb.BindFormat(new OrientAuthenticationURLFormat());
            AuthUrl=cb.Build().GetText();
            cb.BindTokens(TokenRepo.commandUrl); cb.BindFormat(new OrientCommandURLFormat());
            CommandUrl=cb.Build().GetText();
            owm.SetCredentials(
              new NetworkCredential(ConfigurationManager.AppSettings["orient_login"], ConfigurationManager.AppSettings["orient_pswd"]));

            cb.BindTokens(TokenRepo.addDbURL); cb.BindFormat(new OrientDatabaseUrlFormat());
            DatabaseUrl=cb.Build().GetText();
       }

        public void changeAuthCredentials(string Login, string Password)
        {
            owm.SetCredentials(new NetworkCredential(Login, Password));
       }

        public string Add(ITypeToken rest_command_, ITypeToken dbName_, ITypeToken type_)
        {
            List<ITypeToken> commandTk=tb.Command(dbName_, type_);

            cb.BindTokens(commandTk);
            cb.BindFormat(new TextToken() {Text=@"{0}/{1}"});
            string command=cb.Build().GetText();

            QueryUrl=DatabaseUrl + "/" + command;

            wm.SetCredentials(
                new NetworkCredential(ConfigurationManager.AppSettings["orient_login"], ConfigurationManager.AppSettings["orient_pswd"]));
            wm.AddRequest(QueryUrl);
            string resp =
            ir.ReadResponse(
                wm.GetResponse( rest_command_.Text)
            );

            return resp;
       }
        public string Add(IOrientVertex obj_)
        {

            string content=jm.SerializeObject(obj_);
            List<ITypeToken> commandTk=tb.Command(new OrientCreateToken(), tk.Get(obj_), tk.GetBase(obj_), new TextToken() {Text=content});

            cb.BindTokens(commandTk);
            cb.BindFormat(new OrientCreateVertexCluaseFormat());
            string command=cb.Build().GetText();

            QueryUrl=CommandUrl + "/" + command;
            owm.Authenticate(AuthUrl);

            string resp =
            ir.ReadResponse(
                owm.GetResponse(QueryUrl, new POST().Text)
               );

            return resp;

       }
        public string Add(IOrientEdge obj_, IOrientVertex from, IOrientVertex to)
        {

          
            string context=jm.SerializeObject(obj_); 
            
            List<ITypeToken> commandTk=tb.Command(new OrientCreateToken(), tk.Get(obj_), tk.GetBase(obj_)
                , new TextToken() {Text= from.id}, new TextToken() {Text=to.id}, new TextToken() {Text=context});
        
            cb.BindTokens(commandTk);
            cb.BindFormat(new FormatFromListGenerator(new TokenMiniFactory()).FromatFromTokenArray(commandTk));
            string command=cb.Build().GetText();

            QueryUrl=CommandUrl + "/" + command;
            owm.Authenticate(AuthUrl);

            string resp =
            ir.ReadResponse(
                owm.GetResponse(QueryUrl, new POST().Text)
               );

            return resp;

       }
        public string Add(IOrientEdge obj_, ITypeToken from, ITypeToken to)
        {
            from.Text=from.Text.Replace(@"#", "");
            to.Text=to.Text.Replace(@"#", "");

            string content=jm.SerializeObject(obj_);
            List<ITypeToken> commandTk=tb.Command(new OrientCreateToken(), tk.Get(obj_), tk.GetBase(obj_), from, to, new TextToken() {Text=content});

            cb.BindTokens(commandTk);
            cb.BindFormat(new FormatFromListGenerator(new TokenMiniFactory()).FromatFromTokenArray(commandTk));
            string command=cb.Build().GetText();
            QueryUrl=CommandUrl + "/" + command;
            owm.Authenticate(AuthUrl);

            string resp =
            ir.ReadResponse(
                owm.GetResponse(QueryUrl, new POST().Text)
               );

            return resp;

       }
        public string Select(Type object_, ITypeToken condition_)
        {
            string result=null;

            List<ITypeToken> commandTk=tb.Command(new OrientSelectToken(), tk.Get(object_), condition_);

            cb.BindTokens(commandTk);
            cb.BindFormat(new FormatFromListGenerator(new TokenMiniFactory()).FromatFromTokenArray(commandTk));
            string command=cb.Build().GetText();

            QueryUrl=CommandUrl + "/" + command;
            owm.Authenticate(AuthUrl);

            result =
            ir.ReadResponse(
                owm.GetResponse(QueryUrl, new GET().Text)
            );

            return result;

       }
        public IEnumerable<T> Select<T>(Type object_, ITypeToken condition_) where T: class
        {
            IEnumerable<T> result=null;

            List<ITypeToken> commandTk=tb.Command(new OrientSelectToken(), tk.Get(object_), condition_);

            cb.BindTokens(commandTk);
            cb.BindFormat(new FormatFromListGenerator(new TokenMiniFactory()).FromatFromTokenArray(commandTk));
            string command=cb.Build().GetText();

            QueryUrl=CommandUrl + "/" + command;
            owm.Authenticate(AuthUrl);

            string strResult =
            ir.ReadResponse(
                owm.GetResponse(QueryUrl, new GET().Text)
            );
            result=jm.DeserializeFromParentNode<T>(strResult,"result");
            return result;

       }
        public IEnumerable<T> Select<T>(string command_) where T : class
        {
            IEnumerable<T> result=null;

            QueryUrl=CommandUrl + "/" + command_;
            owm.Authenticate(AuthUrl);

            string strResult =
            ir.ReadResponse(
                owm.GetResponse(QueryUrl, new GET().Text)
            );
            result=jm.DeserializeFromParentNode<T>(strResult, "result");
            return result;

       }

        public string Delete(Type type_, ITypeToken condition_)
        {
            string deleteClause;

            List<ITypeToken> commandTk=tb.Command(new OrientDeleteToken(), tk.Get(type_), tk.GetBase(type_));
            List<ITypeToken> whereTk=new List<ITypeToken>() {new OrientWhereToken(), condition_};

            cb.BindTokens(commandTk);
            cb.BindFormat(new OrientDeleteCluaseFormat());
            deleteClause=cb.Build().GetText();


            cb.AddTokens(whereTk);
            cb.AddFormat(new OrientWhereClauseFormat());           
            string whereClause=cb.Build().GetText();

            QueryUrl=CommandUrl + "/" + deleteClause + " " + whereClause;

            owm.Authenticate(AuthUrl);

            string resp =
            ir.ReadResponse(
                owm.GetResponse(QueryUrl, new POST().Text)
               );

            return resp;

       }
        public string Delete(Type type_, IOrientObject from_, IOrientObject to_, ITypeToken condition_)
        {
            string deleteClause;

            List<ITypeToken> commandTk=tb.Command(new OrientDeleteToken(), tk.Get(type_), tk.GetBase(type_));
            List<ITypeToken> whereTk=new List<ITypeToken>() {new OrientWhereToken(), condition_};

            cb.BindTokens(commandTk);
            cb.BindFormat(new OrientDeleteCluaseFormat());
            deleteClause=cb.Build().GetText();

            cb.AddTokens(whereTk);
            cb.AddFormat(new OrientWhereClauseFormat());
            string whereClause=cb.Build().GetText();

            QueryUrl=CommandUrl + "/" + deleteClause + " " + whereClause;

            owm.Authenticate(AuthUrl);

            string resp =
            ir.ReadResponse(
                owm.GetResponse(QueryUrl, new POST().Text)
               );

            return resp;

       }

        public string Add(ITypeToken db_name, ITypeToken command_type)
        {
            string result=null;
            List<ITypeToken> dbCommandUrl=new List<ITypeToken>()
            {
                new OrientHost(),new OrientPort(),new OrientURLDatabaseToken(), db_name,new OrientPlocalToken()
           };

            cb.BindTokens(dbCommandUrl);
            cb.BindFormat(new TextToken() {Text=@"{0}:{1}/{2}/{3}/{4}"});
            string command=cb.Build().GetText();          

            WebRequest wr=WebRequest.Create(command);
            wr.Method=command_type.Text;
            wr.Headers.Add(HttpRequestHeader.Authorization, "Basic " +
                System.Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                 string.Format(@"{0}:{1}",
                ConfigurationManager.AppSettings["orient_login"], ConfigurationManager.AppSettings["orient_pswd"])
                )));

            wr.ContentLength=0;
            wr.Headers.Remove(HttpRequestHeader.Cookie);
            wr.Credentials=null;
            wr.ContentType="text/json";

            try
            {
                result=ir.ReadResponse(wr.GetResponse());
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            return result;
       }

        public string Function(ITypeToken functionName, List<ITypeToken> params_)
        {
            string result=null;
            List<ITypeToken> connectTokens=new List<ITypeToken>() {
                new OrientHost(),new OrientPort(),new OrientFuncionToken()
           };
            List<ITypeToken> functionTokens=new List<ITypeToken>() {};
            functionTokens.Add(functionName);
            foreach (ITypeToken itt in params_)
            {
                functionTokens.Add(itt);
           }

            cb.AddTokens(connectTokens);
            cb.AddFormat(new FormatFromListGenerator(new TokenMiniFactory()).FromatFromTokenArray(connectTokens));
            string connect=cb.Build().GetText();
        
            cb.AddTokens(functionTokens);
            cb.AddFormat(new FormatFromListGenerator(new TokenMiniFactory()).FromatFromTokenArray(functionTokens));
            string command=cb.Build().GetText();         

            string url=connect + "/" + command;

            owm.Authenticate(AuthUrl);
            result=ir.ReadResponse(
                owm.GetResponse(url, new GET().Text)
                );
            return result;
       }

   }

}
