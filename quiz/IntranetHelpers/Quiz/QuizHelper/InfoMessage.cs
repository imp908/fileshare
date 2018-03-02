using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.QuizModel
{
    public class InfoMessage
    {
        public string MessageText { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        Error,
        Success,
        Info,
        Warning
    }

}