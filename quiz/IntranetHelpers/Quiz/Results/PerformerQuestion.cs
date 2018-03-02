using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.QuizModel
{
    public class PerformerQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string YourAnswer { get; set; }
    }
}