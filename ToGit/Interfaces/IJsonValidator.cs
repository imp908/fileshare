using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAPI.Interfaces
{
    public interface IJsonValidator
    {
        bool Validate(string json);
    }
}
