using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.QuizModel
{
    public class PerformerAnswer
    {
        public PerformerQuestion PerformerQuestion { get; set; }
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
    }
}