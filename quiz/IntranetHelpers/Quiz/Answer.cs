using System;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Intranet.Models.QuizModel
{

    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool Checked { get; set; }
        public AnswerType AnswerType { get; set; }
    }

    public enum AnswerType
    {
        //Int,
        //Bool,
        //Date,
        String,
    }

}