using System;
using System.Collections.Generic;

namespace Intranet.Models.QuizModel
{
    public interface IQuiz
    {
        void InsertNewPerformerDataAsync(Quiz quiz);
        void InsertNewPerformerData(Quiz quiz);
        bool IsAlreadyPassed(string id, out DateTime? resolveDate);
        List<QuizResult> SelectResults(string id);
    }
}