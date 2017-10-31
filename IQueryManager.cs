
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IQueryManagers
{
   
    /// <summary>
    /// Main manager interfaces
    /// </summary>

    /// <summary>
    /// Tokens for Orient API URIs 
    /// Different API types tend to different Http req strategies example: Fucntion/param or: Batch/ + JSON-body
    /// (add types to ItypeToken for plugging-in)
    /// </summary>
    //For token items
    public interface ITypeToken
    {
        string Text { get; set; }
    }
    public interface ITokenBuilder
    {

        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType, ITypeToken context_);
        List<ITypeToken> Command(ITypeToken command_, ITypeToken orientObject, ITypeToken orientType, ITypeToken tokenA, ITypeToken tokenB, ITypeToken content );

    }
    //Building Item from Token types
    public interface ITextBuilder
    {
        ITypeToken Text { get; }
        ITypeToken FormatPattern { get; }
        List<ITypeToken> Tokens { get; }

        string Build(List<ITypeToken> tokens_, ITypeToken FormatPattern_);
        string GetText();
        void SetText(List<ITypeToken> tokens_, ITypeToken FormatPattern_);      
        
    }
    
}