using System.Collections.Generic;
using IQueryManagers;

namespace IFormats
{
    public interface IFormatFromListGenerator
    {
        ITypeToken Token { get; set; }

        ITypeToken FormatFromListGenerate(List<ITypeToken> tokens);
        ITypeToken FormatFromListGenerate(List<ITypeToken> tokens, string delimeter);
    }
}