using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orient.Client;

namespace Intranet.Models.QuizModel
{

    public class QuizMainInfo
    {
        public QuizMainInfo() { }

        public ORID ORID { get; set; }
        public string Author { get; set; }
        public string QuizName { get; set; }
        public string QuizDescription { get; set; }
        public QuizType QuizType { get; set; }
        public State State { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public enum QuizType
    {
        Quiz = 1,
        Test = 2
    }

    public enum State
    {
        Finished,
        Published,
        Drafted
    }

}