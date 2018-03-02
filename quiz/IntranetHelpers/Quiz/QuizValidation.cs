using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Intranet.Models.QuizModel
{
    public class ValidateAtLeastOneCheckedAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var app = value as Quiz;
            var inValid = false;
            var p = "Для продолжения заполните обязательные поля: ";
            //item.Answers == null && item.AllowYourAnswer && string.IsNullOrEmpty(item.YourAnswer)
            foreach (var item in app.Questions.AsEnumerable().Reverse())
            {
                if (item.Answers == null && item.AllowYourAnswer && string.IsNullOrEmpty(item.YourAnswer))
                {
                   // p += "Не указан свой ответ вопроса №" + item.Id + ";"; //комментируем по просьбе чилиняк
                   // inValid = true;
                    continue;
                }
                if (item.QuestionType.Require == false) continue;
                if (item.SelectedAnswerId == 0 && !item.Answers.Any(x => x.Checked) &&
                    (item.QuestionType.TypeOfQuestion != TypeOfQuestion.Multiple ||
                     !string.IsNullOrEmpty(item.YourAnswer) || item.SelectedYourAnswer != true))

                    if (string.IsNullOrEmpty(item.YourAnswer) || item.SelectedYourAnswer != true)
                    {
                        p += "Ответ на вопрос №" + item.Id + "; ";
                        inValid = true;
                    }

                if ((item.QuestionType.TypeOfQuestion != TypeOfQuestion.Single ||
                     !string.IsNullOrEmpty(item.YourAnswer) || item.SelectedAnswerId != -1) &&
                    (item.QuestionType.TypeOfQuestion != TypeOfQuestion.Multiple ||
                     !string.IsNullOrEmpty(item.YourAnswer) || item.SelectedYourAnswer != true)) continue;
                p += "Не указан свой ответ вопроса №" + item.Id + ";";
                inValid = true;
            }
            return inValid
                ? new ValidationResult(p)
                : ValidationResult.Success;
        }
    }
}