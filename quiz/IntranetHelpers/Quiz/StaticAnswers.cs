using System.Collections.Generic;
namespace Intranet.Models.QuizModel
{
    public static class StaticAnswers
    {
        public static List<Answer> NpsAnswers(int questionId) => new List<Answer>
        {
            new Answer {AnswerText = "10",AnswerType = AnswerType.String, Id = 1, Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "9", AnswerType = AnswerType.String, Id = 2, Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "8", AnswerType = AnswerType.String, Id = 3, Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "7", AnswerType = AnswerType.String, Id = 4, Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "6", AnswerType = AnswerType.String, Id = 5, Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "5", AnswerType = AnswerType.String, Id = 6, Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "4", AnswerType = AnswerType.String, Id = 7, Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "3", AnswerType = AnswerType.String, Id = 8, Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "2", AnswerType = AnswerType.String, Id = 9,Checked = false, QuestionId = questionId},
            new Answer {AnswerText = "1", AnswerType = AnswerType.String, Id = 10,Checked = false, QuestionId = questionId}
        };
    }

}