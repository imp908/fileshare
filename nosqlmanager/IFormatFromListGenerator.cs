using System.Collections.Generic;
using IQueryManagers;

namespace IFormats
{
    public interface IFormatFromListGenerator
    {    
        ITypeToken FormatFromListGenerate(List<ITypeToken> tokens);
        ITypeToken FormatFromListGenerate(List<ITypeToken> tokens, string delimeter);
        ITypeToken FormatFromListGenerate<T>(List<T> items, string delimeter = null)
            where T : class;
    }
}