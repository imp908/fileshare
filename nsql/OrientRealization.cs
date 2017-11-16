
using System;
using System.Collections.Generic;

using System.Linq;

using System.Net;

using System.Configuration;

using WebManagers;
using IQueryManagers;
using QueryManagers;
using IOrientObjects;

using POCO;

/// <summary>
/// Realization of IJsonMangers, IWebManagers, and IOrient specifically for orient db
/// </summary>
namespace OrientRealization
{

    /// <summary>
    ///     Orient specific WebManager for authentication and authenticated responses sending to URL
    ///     with NetworkCredentials
    /// </summary>    

    public class OrientWebManager : WebManager
    {
        string OSESSIONID = string.Empty;

        //>> add async
        public new HttpWebResponse GetResponse(string url, string method)
        {

            //HttpWebResponse resp = null;
            base.RequestAdd(url, method);
            base.AddHeader(HttpRequestHeader.Cookie, this.OSESSIONID);

            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }

            return null;
        }
        public HttpWebResponse GetResponseCred(string method_)
        {

            //HttpWebResponse resp = null; base.addRequest(url, method);
            this._request.Method = method_;
            try
            {
                return (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }

            return null;
        }
        public WebResponse Authenticate(string url, NetworkCredential nc = null)
        {

            WebResponse resp;
            RequestAdd(url, "GET");
            if (nc == null) { CredentialsBind(); }
            else { AddCredentials(nc); }
            try
            {
                resp = this._request.GetResponse();
                OSESSIONID = GetHeaderValue("Set-Cookie");
                return resp;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }

    #region Tokens

    /// <summary>
    ///  Tokens realization for different string concatenations
    /// </summary>

    //Tokens for Orient Comamnd and Authenticate URLs
    public class OrientHost : ITypeToken
    {
        public string Text { get; set; } = ConfigurationManager.AppSettings["ParentHost"];
    }
    public class OrientDatabaseNameToken : ITypeToken
    {
        public string Text { get; set; } = ConfigurationManager.AppSettings["ParentDBname"];
    }
    public class OrientPort : ITypeToken
    {
        public string Text { get; set; } = "2480";
    }
    public class OrientDatabaseCRUDToken : ITypeToken
    {
        public string Text { get; set; } = "Connect";
    }
    public class OrientAuthenticateToken : ITypeToken
    {
        public string Text { get; set; } = "connect";
    }
    public class OrientCommandToken : ITypeToken
    {
        public string Text { get; set; } = "command";
    }
    public class OrientCommandSQLTypeToken : ITypeToken
    {
        public string Text { get; set; } = "sql";
    }
    public class OrientFuncionToken : ITypeToken
    {
        public string Text { get; set; } = "function";
    }
    public class OrientBatchToken : ITypeToken
    {
        public string Text { get; set; } = "Batch";
    }


    /// <summary>
    /// Orient query tokens
    /// </summary>  

    //Orient SQL syntax tokens
    public class OrientDatabaseToken : ITypeToken
    {
        public string Text { get; set; } = "database";
    }
    public class OrientPlocalToken : ITypeToken
    {
        public string Text { get; set; } = "plocal";
    }

    public class OrientSelectToken : ITypeToken
    {
        public string Text { get; set; } = "Select";
    }
    public class OrientFromToken : ITypeToken
    {
        public string Text { get; set; } = "from";
    }
    public class OrientWhereToken : ITypeToken
    {
        public string Text { get; set; } = "where";
    }
    public class OrientExpandToken : ITypeToken
    {
        public string Text { get; set; } = "expand";
    }

    public class OrientCreateToken : ITypeToken
    {
        public string Text { get; set; } = "Create";
    }
    public class OrientContentToken : ITypeToken
    {
        public string Text { get; set; } = "content";
    }
    public class OrientDeleteToken : ITypeToken
    {
        public string Text { get; set; } = "Delete";
    }
    public class OrientDropToken : ITypeToken
    {
        public string Text { get; set; } = "Drop";
    }

    public class OrientAndToken : ITypeToken
    {
        public string Text { get; set; } = @"and";
    }

    public class OrientClassToken : ITypeToken
    {
        public string Text { get; set; } = "Class";
    }
    public class OrientVertexToken : ITypeToken
    {
        public string Text { get; set; } = "Vertex";
    }
    public class OrientEdgeToken : ITypeToken
    {
        public string Text { get; set; } = "Edge";
    }
    public class OrientPropertyToken : ITypeToken
    {
        public string Text { get; set; } = "Property";
    }
    public class OrientToToken : ITypeToken
    {
        public string Text { get; set; } = "To";
    }
    public class OrientInToken : ITypeToken
    {
        public string Text { get; set; } = "in";
    }
    public class OrientOutToken : ITypeToken
    {
        public string Text { get; set; } = "out";
    }
    public class OrientEToken : ITypeToken
    {
        public string Text { get; set; } = "E";
    }
    public class OrientVToken : ITypeToken
    {
        public string Text { get; set; } = "V";
    }

    public class OrientExtendsToken : ITypeToken
    {
        public string Text { get; set; } = "Extends";
    }
    public class OrientMandatoryToken : ITypeToken
    {
        public string Text { get; set; } = "MANDATORY";
    }
    public class OrientNotNULLToken : ITypeToken
    {
        public string Text { get; set; } = "notnull";
    }

    public class OrientEqualsToken : ITypeToken
    {
        public string Text { get; set; } = @"=";
    }
    public class OrientApostropheToken : ITypeToken
    {
        public string Text { get; set; } = @"'";
    }
    public class OrientDotToken : ITypeToken
    {
        public string Text { get; set; } = @".";
    }
    public class OrientComaToken : ITypeToken
    {
        public string Text { get; set; } = @",";
    }
    public class OrientLtRoundSquareToken : ITypeToken
    {
        public string Text { get; set; } = @"(";
    }
    public class OrientRtRoundSquareToken : ITypeToken
    {
        public string Text { get; set; } = @")";
    }
    public class OrientTRUEToken : ITypeToken
    {
        public string Text { get; set; } = @"TRUE";
    }
    public class OrientFLASEToken : ITypeToken
    {
        public string Text { get; set; } = @"FALSE";
    }

    public class OrientIdSharpToken : ITypeToken
    {
        public string Text { get; set; } = @"#";
    }
    public class OrientIdToken : ITypeToken
    {
        public string Text { get; set; } = "Id";
    }
    public class OrientNameToken : ITypeToken
    {
        public string Text { get; set; } = "Name";
    }
    public class OrientAccountToken : ITypeToken
    {
        public string Text { get; set; } = "sAMAccountName";
    }

    //orient dateformats tokens
    public class OrientSTRINGToken : ITypeToken
    {
        public string Text { get; set; } = "STRING";
    }
    public class OrientDATEToken : ITypeToken
    {
        public string Text { get; set; } = "DATE";
    }
    public class OrientINTEGERToken : ITypeToken
    {
        public string Text { get; set; } = "INTEGER";
    }
    public class OrientDATETIMEToken : ITypeToken
    {
        public string Text { get; set; } = "DATETIME";
    }

    public class OrientTestDbToken : ITypeToken
    {
        public string Text { get; set; } = "TestDB";
    }

    public class OrientUserSettingsToken : ITypeToken
    {
        public string Text { get; set; } = "UserSettings";
    }
    public class OrientCommonSettingsToken : ITypeToken
    {
        public string Text { get; set; } = "CommonSettings";
    }
    //orient Class tokens
    public class OrientPersonToken : ITypeToken
    {
        public string Text { get; set; } = "Person";
    }
    public class OrientUnitToken : ITypeToken
    {
        public string Text { get; set; } = "Unit";
    }
    public class OrientSubUnitToken : ITypeToken
    {
        public string Text { get; set; } = "SubUnit";
    }
    public class OrientMainAssignmentToken : ITypeToken
    {
        public string Text { get; set; } = "MainAssignment";
    }
    public class OrientTrackBirthdaysToken : ITypeToken
    {
        public string Text { get; set; } = "TrackBirthdays";
    }

    public class OrientBrewery : ITypeToken
    {
        public string Text { get; set; } = "Brewery";
    }


    ///<summary>
    ///Web response reader Tokens
    /// </summary>

    public class RESULT : ITypeToken
    {
        public string Text { get; set; } = "result";
    }
    #endregion

    #region TokenFormats
    /// <summary>
    /// Builder formats
    /// </summary>

    //genrate sample format from colectin of tokens
    public class OrientTokenFormatFromListGenerate : ITypeToken
    {
        public string Text { get; set; }
        public OrientTokenFormatFromListGenerate(List<ITypeToken> tokens)
        {
            string res = "{}";
            int[] cnt = new int[tokens.Count];
            for (int i = 0; i < tokens.Count(); i++)
            {
                cnt[i] = i;
            }
            string res2 = string.Join(@"} {", cnt);
            this.Text = res.Insert(1, res2);
        }
        public OrientTokenFormatFromListGenerate(List<ITypeToken> tokens, string delimeter)
        {
            string res = "{}";
            string placeholder = string.Format("}", delimeter, "{");
            int[] cnt = new int[tokens.Count];
            for (int i = 0; i < tokens.Count(); i++)
            {
                cnt[i] = i;
            }
            string res2 = string.Join(placeholder, cnt);
            this.Text = res.Insert(1, res2);
        }
    }
    //Auth Orient URL
    public class OrientAuthenticationURLFormat : ITypeToken
    {
        public string Text { get { return @"{0}:{1}/{2}/{3}"; } set { Text = value; } }
    }
    //Command URL part format
    public class OrientCommandURLFormat : ITypeToken
    {
        public string Text { get { return @"{0}:{1}/{2}/{3}/{4}"; } set { Text = value; } }
    }
    public class OrientDatabaseUrlFormat : ITypeToken
    {
        public string Text { get { return @"{0}/{1}"; } set { Text = value; } }
    }


    /// </summary>
    /// command queries contains prevoius command as first parameter, 
    /// cause WHERE not intended to be used without select
    /// </summary>

    //command query part format
    public class OrientSelectClauseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1} {2}"; } set { Text = value; } }
    }
    //Command for concatenating select command and where clause
    public class OrientWhereClauseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1}"; } set { Text = value; } }
    }
    //create vertex command Format
    public class OrientCreateVertexCluaseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1} {2} {3} {4}"; } set { Text = value; } }
    }
    //delete vertex command Format
    public class OrientDeleteVertexCluaseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1} {2} {3}"; } set { Text = value; } }
    }
    //delete command Format
    public class OrientDeleteCluaseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1} {2}"; } set { Text = value; } }
    }
    //format for inEoutV select
    public class OrientOutEinVFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1}{2} {3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18} {19} {20} {21} {22}"; } set { Text = value; } }
    }
    public class OrientOutVinVFormat : ITypeToken
    {
        public string Text { get { return @"{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12} {13} {14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}"; } set { Text = value; } }
    }
    #endregion

    #region TokenListsBuilders

    //buider for commands with format
    //mostly used for URLS (auth,
    public class OrientCommandBuilder : CommandBuilder
    {
        public OrientCommandBuilder() : base()
        {

        }
        public OrientCommandBuilder(List<ITypeToken> tokens_, ITypeToken FormatPattern_)
             : base(tokens_, FormatPattern_)
        {

        }
    }

    //<<--deprecation possible, replaced with type convertible commandbuilder
    //Query builders
    //class segregation for different cluse builders

    //Authentication URL build
    public class OrientAuthenticationURIBuilder : CommandBuilder
    {
        public OrientAuthenticationURIBuilder(List<ITypeToken> tokens_, OrientAuthenticationURLFormat FormatPattern_)
             : base(tokens_, FormatPattern_)
        {

        }
    }
    //Command URL build
    public class OrientCommandURIBuilder : CommandBuilder
    {
        public OrientCommandURIBuilder(List<ITypeToken> tokens_, OrientCommandURLFormat FormatPattern_)
            : base(tokens_, FormatPattern_)
        {

        }
        public OrientCommandURIBuilder(List<ICommandBuilder> texts_, ITypeToken FormatPattern_, CommandBuilder.BuildTypeFormates type_)
          : base(texts_, FormatPattern_, type_)
        {

        }
    }

    public class OrientSelectClauseBuilder : CommandBuilder
    {
        public OrientSelectClauseBuilder(List<ITypeToken> tokens_, OrientSelectClauseFormat FormatPattern_ = null)
            : base(tokens_, FormatPattern_ = new OrientSelectClauseFormat())
        {

        }
    }
    public class OrientWhereClauseBuilder : CommandBuilder
    {
        public OrientWhereClauseBuilder(List<ITypeToken> tokens_, OrientWhereClauseFormat FormatPattern_)
            : base(tokens_, FormatPattern_)
        {

        }
    }

    public class OrientCreateClauseBuilder : CommandBuilder
    {
        public OrientCreateClauseBuilder(List<ITypeToken> tokens_, ITypeToken format_)
            : base(tokens_, format_)
        {

        }
    }
    public class OrientDeleteClauseBuilder : CommandBuilder
    {
        public OrientDeleteClauseBuilder(List<ITypeToken> tokens_, ITypeToken format_)
            : base(tokens_, format_)
        {

        }
    }

    public class OrientNestedSelectClauseBuilder : CommandBuilder
    {
        public OrientNestedSelectClauseBuilder(List<ITypeToken> tokens_, ITypeToken format_)
            : base(tokens_, format_)
        {

        }
    }
    #endregion

    //predefined url token collections
    //prefered change to predefined url and command builds
    public static class TokenRepo
    {
        public static List<ITypeToken> authUrl = new List<ITypeToken>() { new OrientHost(), new OrientPort(), new OrientAuthenticateToken(), new OrientDatabaseNameToken() };
        public static List<ITypeToken> commandUrl = new List<ITypeToken>() { new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDatabaseNameToken(), new OrientCommandSQLTypeToken() };
        public static List<ITypeToken> addDbURL = new List<ITypeToken>() { new OrientHost(), new OrientPort(), new OrientDatabaseToken()};
    }   
	
    #region CommandBuilders
	
   /// <summary>
    /// Builders.
    /// Build command acording to type of passed object (class,vertes, or edge with objects referenced or ids)
    /// Not use predefined formatters 
    /// for special commands like create class/edge/vertex
    /// which not requer special format like {0}:{1}\{2} but samle , generated fro mtoken list like {0} {1} {2}
    /// but generated in lagre ammounts with differen types.
    /// </summary>
    public class OrientTokenBuilder : ITokenBuilder
    {
        //for select command
        public List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            result.Add(new OrientFromToken());
            result.Add(orientObject);
            return result;
        }

        //for delete, select conditional command
        public List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            if (command_ is OrientDeleteToken)
            {
                result.Add(command_);
                result.Add(orientType);
                //result.Add(new OrientFromToken());
                result.Add(orientObject);
            }
            if (command_ is OrientSelectToken)
            {
                result.Add(command_);
                result.Add(new OrientFromToken());
                result.Add(orientObject);                
                result.Add(new OrientWhereToken());
                result.Add(orientType);
            }
            return result;
        }

        //for create command, authenticate, command
        public List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType, ITypeToken context_)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            result.Add(orientType);
            result.Add(orientObject);
            result.Add(new OrientContentToken());
            result.Add(context_);
            return result;
        }

        //for create Edge from to 
        public List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType,  ITypeToken tokenA, ITypeToken tokenB, ITypeToken context_)
        {
            tokenA.Text = tokenA.Text.Replace(@"#", "");
            tokenB.Text = tokenB.Text.Replace(@"#", "");

            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            result.Add(orientType);
            result.Add(orientObject);
            result.Add(new OrientFromToken());
            result.Add(tokenA);
            result.Add(new OrientToToken());
            result.Add(tokenB);

            if(command_ is OrientCreateToken){
                if (context_ != null && context_.Text != null && context_.Text != string.Empty)
                {
                    result.Add(new OrientContentToken());
                    result.Add(context_);
                }
            }
            if (command_ is OrientDeleteToken)
            {
                if (context_ != null && context_.Text != null && context_.Text != string.Empty)
                {                    
                    result.Add(context_);
                }
            }
            return result;
        }        

    }

    public class OreintNewsTokenBuilder
    {
        TypeConverter typeConverter_ = new TypeConverter();

        public List<ITypeToken> outEinVExp(ITypeToken command_,ITypeToken vertex_,ITypeToken edge_, ITypeToken condition_)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            //expand
            result.Add(new OrientExpandToken());
            result.Add(new OrientLtRoundSquareToken());
            
            //outE
            result.Add(new OrientOutToken());
            result.Add(new OrientEToken());
            result.Add(new OrientLtRoundSquareToken());
            result.Add(new OrientApostropheToken());
            result.Add(edge_);
            result.Add(new OrientApostropheToken());
            result.Add(new OrientRtRoundSquareToken());
            result.Add(new OrientDotToken());
            //inv
            result.Add(new OrientInToken());
            result.Add(new OrientVToken());
            result.Add(new OrientLtRoundSquareToken());
            result.Add(new OrientApostropheToken());
            result.Add(vertex_);
            result.Add(new OrientApostropheToken());
            result.Add(new OrientRtRoundSquareToken());

            result.Add(new OrientRtRoundSquareToken());

            result.Add(new OrientFromToken());
            result.Add(vertex_);

            if(condition_!= null && condition_.Text != null && condition_.Text!=string.Empty)
            {
                result.Add(new OrientWhereToken());
                result.Add(condition_);
            }

            result.Add(new OrientLtRoundSquareToken());
            result.Add(new OrientApostropheToken());
            result.Add(new OrientRtRoundSquareToken());
            return result;
        }

        public List<ITypeToken> outVinVcnd(Type object_, ITypeToken property_, ITypeToken conda_, ITypeToken condb_)
        {
            List<ITypeToken> result = new List<ITypeToken>();         
            
            if (object_.BaseType == typeof(OrientVertex) || object_.BaseType == typeof(OrientEdge))
            {
                       
                //outV
                result.Add(new OrientOutToken());
                result.Add(new OrientVToken());
                result.Add(new OrientLtRoundSquareToken());
                result.Add(new OrientApostropheToken());
                result.Add(typeConverter_.Get(object_));
                result.Add(new OrientApostropheToken());
                result.Add(new OrientRtRoundSquareToken());
                result.Add(new OrientDotToken());
                result.Add(property_);

                result.Add(new OrientEqualsToken());

                result.Add(new OrientApostropheToken());
                result.Add(conda_);
                result.Add(new OrientApostropheToken());

                result.Add(new OrientAndToken());

                //inv
                result.Add(new OrientInToken());
                result.Add(new OrientVToken());
                result.Add(new OrientLtRoundSquareToken());
                result.Add(new OrientApostropheToken());
                result.Add(typeConverter_.Get(object_));
                result.Add(new OrientApostropheToken());
                result.Add(new OrientRtRoundSquareToken());
                result.Add(new OrientDotToken());
                result.Add(property_);

                result.Add(new OrientEqualsToken());

                result.Add(new OrientApostropheToken());
                result.Add(condb_);
                result.Add(new OrientApostropheToken());

            }
            return result;
        }

    }
	/// <summary>creates collection of tokens
    /// builds add,delete,create commands from token amount
    /// </summary>
    public class OrientCommandBuilderImplicit : ITokenBuilderTypeGen
    {

        ITypeConverter _typeConverter;

        public void BindConverter(ITypeConverter typecOnverter_)
        {
            this._typeConverter = typecOnverter_;
        }

        public List<ITypeToken> Command(ITypeToken name_, ITypeToken type_)
        {
            List<ITypeToken> result = new List<ITypeToken>();                           
                result.Add(name_);
                result.Add(type_);
            return result;
        }


        public List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            result.Add(_typeConverter.GetBase(orientClass_.GetType()));
            result.Add(_typeConverter.Get(orientClass_.GetType()));


            return result;
        }


        public List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken content = null)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            result.Add(_typeConverter.GetBase(orientClass_.GetType()));
            result.Add(_typeConverter.Get(orientClass_.GetType()));
            if (content != null)

            {
                if (orientClass_ is IOrientClass)
                {
                    result.Add(new OrientExtendsToken());
                }
                if (orientClass_ is IOrientVertex)
                {
                    result.Add(new OrientContentToken());
                }



                result.Add(content);
            }
            return result;
        }
        public List<ITypeToken> Command(ITypeToken command_, Type orientClass_, ITypeToken content = null)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            result.Add(_typeConverter.GetBase(orientClass_.GetType()));
            result.Add(_typeConverter.Get(orientClass_.GetType()));
            if (content != null)

            {
                if (_typeConverter.GetBase(orientClass_.GetType()) is IOrientClass)
                {
                    result.Add(new OrientExtendsToken());
                }
                if (_typeConverter.GetBase(orientClass_.GetType()) is IOrientVertex)
                {
                    result.Add(new OrientContentToken());
                }




                result.Add(content);
            }
            return result;
        }


        public List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, IOrientObject orientProperty_, ITypeToken orientType_, bool mandatory = false, bool notnull = false)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            result.Add(_typeConverter.GetBase(orientProperty_.GetType()));
            result.Add(_typeConverter.Get(orientClass_.GetType()));
            result.Add(new OrientDotToken());
            result.Add(_typeConverter.Get(orientProperty_.GetType()));
            result.Add(orientType_);
            result.Add(new OrientLtRoundSquareToken());
            result.Add(new OrientMandatoryToken());
            if (mandatory)
            {
                result.Add(new OrientTRUEToken());
            }
            else { result.Add(new OrientFLASEToken()); }
            result.Add(new OrientNotNULLToken());

            result.Add(new OrientComaToken());

            if (notnull)
            {
                result.Add(new OrientTRUEToken());
            }
            else { result.Add(new OrientFLASEToken()); }
            result.Add(new OrientRtRoundSquareToken());





            return result;
        }


        public List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken from, ITypeToken to, ITypeToken content = null)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            result.Add(command_);
            result.Add(_typeConverter.GetBase(orientClass_.GetType()));
            result.Add(_typeConverter.Get(orientClass_.GetType()));


            result.Add(new OrientFromToken());
            result.Add(from);
            result.Add(new OrientToToken());
            result.Add(to);
            if (content != null)
            {
                result.Add(new OrientContentToken());
                result.Add(content);
            }

            return result;
        }

    }
	
    /// <summary>
    /// Builder with exlicitly named commands
    /// </summary>
    public class OrientTokenBuilderExplicit
    {
        
        //Create class cluase (type check) with extends class option
        public List<ITypeToken> Create(OrientClassToken classType_, ITypeToken extendsClassType_=null)
        {

            List<ITypeToken> result = new List<ITypeToken>() {
                new OrientCreateToken()
            };

            if (classType_ is OrientClassToken)
            {
                result.Add(new OrientClassToken());
                result.Add(classType_);

                if (extendsClassType_ != null)
                {
                    result.Add(new OrientExtendsToken());
                    result.Add(extendsClassType_);
                }
            }           
            return result;
        }
        //Create  vertex cluase (type check) with content optiona
        public List<ITypeToken> Create(OrientVertexToken classType_, ITypeToken extendsClassType_=null)
        {
            List<ITypeToken> result = new List<ITypeToken>() {
                new OrientCreateToken()
            };
            if (classType_ is OrientVertexToken)
            {
                result.Add(new OrientVertexToken());
                result.Add(classType_);

                if (extendsClassType_ != null)
                {
                    result.Add(new OrientContentToken());
                    result.Add(extendsClassType_);
                }
            }
            return result;
        }
        //Create property cluase
        public List<ITypeToken> Create(ITypeToken className_, ITypeToken propertyName_, ITypeToken propertyType_,
        bool mandatory_ = false, bool notnull=false)
        {
            List<ITypeToken> result = new List<ITypeToken>() {
                new OrientCreateToken(), new OrientPropertyToken(),className_, new OrientDotToken(),propertyName_,propertyType_
            };

            result.Add(new OrientLtRoundSquareToken());
            result.Add(new OrientMandatoryToken());

            if (mandatory_){
                result.Add(new OrientTRUEToken());
            }else { result.Add(new OrientFLASEToken()); }

            result.Add(new OrientComaToken());
            result.Add(new OrientNotNULLToken());

            if (notnull){
                result.Add(new OrientTRUEToken());
            }else { result.Add(new OrientFLASEToken()); }

            result.Add(new OrientRtRoundSquareToken());

            return result;
        }
        //Create edge clause
        public List<ITypeToken> Create (ITypeToken className_, ITypeToken from_,ITypeToken to_)
        {

            List<ITypeToken> result = new List<ITypeToken>(){
                new OrientCreateToken(), new OrientEdgeToken(), className_, new OrientFromToken(),
                from_,
                new OrientToToken(),
                to_
            };

            return result;
        }
        //For select from cluases (Vertex,edge)
        public List<ITypeToken> Select(ITypeToken orientType_, ITypeToken class_)
        {
            List<ITypeToken> result = new List<ITypeToken>() {
                new OrientSelectToken(),orientType_, new OrientFromToken(), class_
            };

            return result;
        }
        //Where clauses receiveing content< which must be strictly parametrized in UOW
        public List<ITypeToken> Where(ITypeToken orientType_, ITypeToken content_)
        {
            List<ITypeToken> result = new List<ITypeToken>() {
                new OrientWhereToken(),content_
            };

            return result;
        }
        //delete vertex,edge,or class
        public List<ITypeToken> Delete(ITypeToken classType_,ITypeToken classname_)
        {
            List<ITypeToken> result = new List<ITypeToken>();
            if (classType_ is OrientVertexToken)
            {
                result.Add(new OrientDeleteToken());
                result.Add(classType_);
            }
            if (classType_ is OrientEdgeToken)
            {
                result.Add(new OrientDeleteToken());
                result.Add(classType_);
            }
            if (classType_ is OrientClassToken)
            {
                result.Add(new OrientDropToken());
                result.Add(classType_);
            }
            if(result.Count()!=0)
            {
                result.Add(classname_);
            }
            return result;
        }

        public List<ITypeToken> Function(ITypeToken function_,ICommandBuilder params_)
        {
            List<ITypeToken> result = new List<ITypeToken>();
                result.Add(function_);
                result.AddRange(params_.Tokens);
            return result;
        }
    }
	
	#endregion	

    ///<summary>Converts from model poco classes types to ItypeToken types
    ///</summary>
    public class TypeConverter : ITypeConverter
    {

        Dictionary<Type, ITypeToken> types;

        public TypeConverter()
        {

            types = new Dictionary<Type, ITypeToken>();

            types.Add(typeof(OrientVertex), new OrientVertexToken());
            types.Add(typeof(OrientEdge), new OrientEdgeToken());

            types.Add(typeof(Person), new OrientPersonToken());
            types.Add(typeof(Unit), new OrientUnitToken());

            types.Add(typeof(MainAssignment), new OrientMainAssignmentToken());
            types.Add(typeof(SubUnit), new OrientSubUnitToken());

            types.Add(typeof(UserSettings), new OrientUserSettingsToken());
            types.Add(typeof(CommonSettings), new OrientCommonSettingsToken());
            
            types.Add(typeof(TrackBirthdays), new OrientTrackBirthdaysToken());
        }
        public void Add(Type type_, ITypeToken token_)
        {
            this.types.Add(type_, token_);
        }
        public ITypeToken Get(Type type_)
        {
            ITypeToken token_ = null;

            types.TryGetValue(type_, out token_);

            return token_;
        }
        public ITypeToken GetBase(Type type_)
        {
            ITypeToken token_ = null;
            Type tp = type_.BaseType;
            types.TryGetValue(tp, out token_);

            return token_;
        }
        public ITypeToken Get(IOrientObject object_)
        {
            ITypeToken token_ = null;

            types.TryGetValue(object_.GetType(), out token_);

            return token_;
        }
        public ITypeToken GetBase(IOrientObject object_)
        {
            ITypeToken token_ = null;
            Type tp = object_.GetType().BaseType;
            IOrientVertex t2 = (IOrientVertex)object_;

            types.TryGetValue(object_.GetType().BaseType, out token_);

            return token_;
        }

    }


}