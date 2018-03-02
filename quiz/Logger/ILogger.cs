using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.Logger
{
    public interface ILogger
    {
        void LogInformation(string infoString);
        void LogError(string errorString);
    }
}