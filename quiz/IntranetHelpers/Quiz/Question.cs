using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orient.Client;

namespace Intranet.Models.QuizModel
{
    public class Question
    {
        public int Id { get; set; }
        public string QuizId { get; set; }
        public int SelectedAnswerId { get; set; }
        public string SelectedAnswerText { get; set; }
        public List<Answer> Answers { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionText { get; set; }
        public List<QuestionPictures> QuestionPictures { get; set; }
        public bool AllowYourAnswer { get; set; }
        public bool SelectedYourAnswer { get; set; }
        public string YourAnswer { get; set; }
    }
}