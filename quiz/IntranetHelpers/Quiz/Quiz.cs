using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Intranet.Models.QuizModel
{
    [ValidateAtLeastOneChecked]
    public class Quiz
    {
        public Quiz() { }

        public QuizMainInfo QuizMainInfo { get; set; }
        public List<Question> Questions { get; set; }
    }


   
}






