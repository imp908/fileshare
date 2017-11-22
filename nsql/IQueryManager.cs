
using System;
using System.Collections.Generic;

using IOrientObjects;
using IFormats;
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
        string Text { get; set; }
    }

    //Building Item from Token types
    public interface ICommandBuilder
    {
        IFormatFromListGenerator formatGenerator { get; }
        ITypeToken typeToken { get; }
        ITypeToken Text { get; }
        ITypeToken FormatPattern { get; }
        List<ITypeToken> Tokens { get; }

        void BindTokens(List<ITypeToken> tokens_);
        void AddTokens(List<ITypeToken> tokens_);
        void BindFormat(ITypeToken formatPatern_);
        void AddFormat(ITypeToken formatPatern_);
        void BindFormatGenerator(IFormatFromListGenerator formatGenerator_);
        void AddBuilders(List<ICommandBuilder> texts_, ITypeToken FormatPattern_ = null);

        string Build();
        string Build(List<ICommandBuilder> tokens_, ITypeToken FormatPattern_);
        string GetText();
        void SetText(List<ITypeToken> tokens_, ITypeToken FormatPattern_);

    }

    /// <summary>
    /// Converts from Database and POCO classes to Tokens
    /// </summary>
    public interface ITypeConverter
    {
        void Add(Type type_, ITypeToken token_);
        ITypeToken Get(IOrientObject object_);
        ITypeToken GetBase(IOrientObject object_);
        ITypeToken Get(Type type_);
        ITypeToken GetBase(Type type_);
    }

    
    public interface ITokenBuilder
    {

        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType, ITypeToken context_);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType, ITypeToken tokenA, ITypeToken tokenB, ITypeToken content );       

    }
    public interface ITokenBuilderNoGen
    {

        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken orientObjectToken_, ITypeToken content = null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken orientObject, ITypeToken from, ITypeToken to, ITypeToken content = null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken orientObject);

    }
    public interface ITokenBuilderTypeGen
    {
        List<ITypeToken> Command(ITypeToken name_, ITypeToken type_);
        List<ITypeToken> Command(ITypeToken command_, Type orientClass_, ITypeToken content = null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_, ITypeToken content = null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_,  ITypeToken from, ITypeToken to, ITypeToken content = null);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_);
        List<ITypeToken> Command(ITypeToken command_, IOrientObject orientClass_,IOrientObject orientProperty_, ITypeToken orientType_, bool mandatory =false, bool notnull=false);
    }
   
}