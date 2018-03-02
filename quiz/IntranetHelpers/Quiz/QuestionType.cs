using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.QuizModel
{
    public class QuestionType
    {
        public bool Require { get; set; }
        public TypeOfQuestion TypeOfQuestion { get; set; }
        public Kind Kind { get; set; }
    }
    public enum TypeOfQuestion
    {
        Single = 1,
        Multiple = 2
    }

    public enum Kind
    {
        Free = 1,
        Rating = 2,
        OnlyYourVariant = 3
    }
}