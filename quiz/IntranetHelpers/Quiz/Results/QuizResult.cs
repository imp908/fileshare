using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.QuizModel
{
    public class QuizResult
    {
        public string QuizId { get; set; }
        public DateTime FinishDate { get; set; }
        public string PerformerName { get; set; }
        public List<PerformerAnswer> PerformerAnswer { get; set; }
    }
}

