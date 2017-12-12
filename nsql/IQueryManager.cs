
using System;
using System.Collections.Generic;

using IOrientObjects;

namespace IQueryManagers
{

    /// <summary>
    /// Main manager interfaces
    /// </summary>

    //For token items
    /// <summary>
    /// Tokens for Orient API URIs 
    /// Different API types tend to different Http req strategies example: Fucntion/param or: Batch/ + JSON-body
    /// (add types to ItypeToken for plugging-in)
    /// </summary>  
    public interface ITypeToken
    {
        string Text {get; set;}      
    }

    public interface ITokenMiniFactory
    {
        ITypeToken NewToken(string text_=null);
        ITypeToken EmptyString();

        ITypeToken Dot();
        ITypeToken Coma();
        ITypeToken Gap();
    }
    
    public interface IOrientQueryFactory
    {
        ITypeToken ClassToken();
        ITypeToken ContentToken();
        ITypeToken CreateToken();
        ITypeToken EdgeToken();
        ITypeToken Equals();
        ITypeToken ExtendsToken();
        ITypeToken FromToken();
        ITypeToken LeftRoundBraket();
        ITypeToken LeftSquareBraket();
        ITypeToken Mandatory();
        ITypeToken NotNull();
        ITypeToken PropertyItemFormatToken();
        ITypeToken PropertyToken();
        ITypeToken PropertyTypeToken();
        ITypeToken RightRoundBraket();
        ITypeToken RightSquareBraket();
        ITypeToken SelectToken();
        ITypeToken ToToken();
        ITypeToken VertexToken();
        ITypeToken WhereToken();
    }
    public interface IOrientBodyFactory
    {

        ITypeToken BackSlash();
        ITypeToken Slash();
        ITypeToken Colon();
        ITypeToken Comma();
        ITypeToken Batch();
        ITypeToken Command();
        ITypeToken PLocal();
        ITypeToken Database();
        ITypeToken Connect();
        ITypeToken Content();
        ITypeToken Language();
        ITypeToken sql();
        ITypeToken LeftFgGap();
        ITypeToken RightFgGap();
        ITypeToken LeftSqGap();
        ITypeToken RightSqGap();
        ITypeToken Operations();
        ITypeToken Quotes();
        ITypeToken Sctipt();
        ITypeToken Transactions();
        ITypeToken True();
        ITypeToken False();
        ITypeToken Type();

        ITypeToken StringToken();
        ITypeToken BooleanToken();

    }

    public interface ICommandFactory
    {
        ICommandBuilder CommandBuilder(ITokenMiniFactory tokenFactory_, IFormatFactory formatFactory_);

        ICommandBuilder CommandBuilder(ITokenMiniFactory tokenFactory_, IFormatFactory formatFactory_
            , List<ITypeToken> tokens_, ITypeToken format_);


    }

    public interface IFormatFactory
    {
        IFormatFromListGenerator FormatGenerator(ITokenMiniFactory tokkenFactory_);
    }

    public interface IFormatFromListGenerator
    {
        ITypeToken FromatFromTokenArray(List<ITypeToken> tokens_, ITypeToken delimeter_=null);
        ITypeToken FromatFromTokenArray(List<ICommandBuilder> tokens_, ITypeToken delimeter_=null);

    }

    //Building Item from Token types
    public interface ICommandBuilder
    {
        IFormatFromListGenerator formatGenerator {get;}
        ITypeToken typeToken {get;}
        ITypeToken Text {get;}
        ITypeToken FormatPattern {get; set;}
        List<ITypeToken> Tokens {get;}

        void BindTokens(List<ITypeToken> tokens_);
        void AddTokens(List<ITypeToken> tokens_);
        void BindFormat(ITypeToken formatPatern_);
        void AddFormat(ITypeToken formatPatern_);
        void BindFormatGenerator(IFormatFromListGenerator formatGenerator_);

        void BindBuilders(List<ICommandBuilder> texts_, ITypeToken FormatPattern_=null);

        ICommandBuilder Build();
        string Build(List<ICommandBuilder> tokens_, ITypeToken FormatPattern_);
        string GetText();
        void SetText(List<ITypeToken> tokens_, ITypeToken FormatPattern_);
    }

    /// <summary>
    /// Converts from Database and POCO classes to Tokens
    /// </summary>
    public interface ITypeTokenConverter
    {
        void Add(Type type_, ITypeToken token_);
        ITypeToken Get(IOrientObject object_);
        ITypeToken GetBase(IOrientObject object_);
        ITypeToken Get(Type type_);
        ITypeToken GetBase(Type type_);
        ITypeToken Get(string object_);
        ITypeToken GetBase(string object_);

        Type GegtypeFromAsm(string typeName_, string asm_ = null);
    }

    public interface IPropertyConverter
    {      
        ITypeToken GetBoolean(bool bool_);
        ITypeToken Get(Type type_);
    }

    //<<< obsolette
    public interface ITokenBuilder
    {

        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType, ITypeToken context_);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType, ITypeToken tokenA, ITypeToken tokenB, ITypeToken content );       

    }
    public interface ITokenBuilderNoGen
    {

        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken orientObjectToken_, ITypeToken content=null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken orientObject, ITypeToken from, ITypeToken to, ITypeToken content=null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken orientObject);

    }
    public interface ITokenBuilderTypeGen
    {
        List<ITypeToken> Command(ITypeToken name_, ITypeToken type_);
        List<ITypeToken> Command(ITypeToken command_, Type orientClass_, ITypeToken content=null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken content=null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_,  ITypeToken from, ITypeToken to, ITypeToken content=null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_,IOrientObject orientProperty_, ITypeToken orientType_, bool mandatory =false, bool notnull=false);
    }
   
}