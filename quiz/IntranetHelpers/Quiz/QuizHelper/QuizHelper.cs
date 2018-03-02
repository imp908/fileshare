using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Orient.Client;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Threading.Tasks;
using Intranet.Controllers;

namespace Intranet.Models.QuizModel
{
    public class QuizHelper : IntranetHelper<Quiz>, IQuiz
    {
        private readonly Quiz Quiz;
        private const string FilePath = @"\\MSK1-VM-INAPP01.nspk.ru\Quiz\UploadFileBase64demo\";// @"\\msk1-vm-ovisp01.nspk.ru\E$\StaticContent\UploadFileBase64demo\";
        private const string NetworkPath = @"http://my.nspk.ru/UploadFileBase64demo/" ; //@"http://static.nspk.ru/UploadFileBase64demo/";
        public QuizHelper(IDatabase database) 
            : base(database)
        {
        }

        public QuizHelper(Quiz quiz, IDatabase database) 
            : base(quiz,database)
        {
            Quiz = base.GeneralElement;//.GetPictures();
        }

        private void PrepareQuizToInsert(string rid)
        {

            foreach (var item in Quiz.Questions)
            {
                item.QuizId = rid;

                if (item.QuestionType.Kind == Kind.Rating)
                {
                    item.QuestionType.Require = true;
                    item.QuestionType.TypeOfQuestion = TypeOfQuestion.Single;
                    item.QuizId = rid;
                    item.Answers = StaticAnswers.NpsAnswers(item.Id);
                }

                if (item.QuestionType.Kind == Kind.OnlyYourVariant)
                {
                    item.AllowYourAnswer = true;
                    item.Answers = new List<Answer>();
                }
                if (item.QuestionType.TypeOfQuestion == TypeOfQuestion.Multiple)
                {
                    item.QuestionType.Kind = Kind.Free;
                }
                if (item.Answers == null) item.Answers = new List<Answer>();
            }
            
            SavePictures();
        }


        public sealed override void Update()
        {
            ODatabase.Update(Quiz.QuizMainInfo.ORID).Set(Quiz.QuizMainInfo).Run();
            ODatabase.Delete.Vertex("Question").Where("QuizId=" + Quiz.QuizMainInfo.ORID.RID).Run();
            PrepareQuizToInsert(Quiz.QuizMainInfo.ORID.RID);
            InsertQuestions();
        }

        public sealed override void Insert()
        {
            if (Quiz.QuizMainInfo.ORID != null)
            {
                Update();
            }
            else
            {
                Quiz.QuizMainInfo.Author = UserName;
                Quiz.QuizMainInfo.CreationDate = DateTime.Now;
                Quiz.QuizMainInfo.State = State.Drafted;

                PrepareQuizToInsert(InsertNewQuizData(Quiz.GetType().Name, Quiz.QuizMainInfo));

                #region vertexIsiscation

                //var mainInfoVertext = ODatabase.Create.Vertex("Quiz").Set(Quiz.QuizMainInfo).Run();

                //foreach (var Question in Quiz.Questions)
                //{
                //    var l = ODatabase.Create.Vertex(Question).Run();
                //    ODatabase.Create.Edge("QuizData").From(mainInfoVertext.ORID).To(l.ORID).Run();
                //}

                //var k = ODatabase.Select().From("(traverse * from #27:2)").ToList<Quiz>();

                #endregion

                InsertQuestions();

                //new JavaScriptSerializer().Serialize(item));
            }
        }

        private void InsertQuestions()
        {
            var orderQuestion = 1;
            Quiz.Questions.Where(x => x.Id != 0).OrderBy(y => y.Id).ToList().AddRange(Quiz.Questions.Where(x => x.Id == 0).ToList());

            foreach (var question in Quiz.Questions ?? new List<Question>())
            {
                var orderAnswer = 1;

                question.Id = orderQuestion;
                foreach (var answer in question.Answers ?? new List<Answer>())
                {
                    answer.QuestionId = question.Id;
                    answer.Id = orderAnswer;
                    orderAnswer++;
                }
                InsertNewQuizData("Question", question);
                orderQuestion++;
            }
        }


        private Quiz GetSelectedAnswerText(Quiz quiz)
        {
            foreach (var item in quiz.Questions.Where(x => x.SelectedAnswerId != 0).ToList())
                if (item.Answers != null) 
                foreach (var answer in item.Answers.Where(x => x.Id == item.SelectedAnswerId).ToList())
                    item.SelectedAnswerText = answer.AnswerText;
            return quiz;
        }


        public async void InsertNewPerformerDataAsync(Quiz quiz)
        { 
            await Task.Run(() =>
            {
                var answers = new List<PerformerAnswer>();
                foreach (var item in GetSelectedAnswerText(quiz).Questions)
                {
                    if (item.QuestionType.TypeOfQuestion == TypeOfQuestion.Multiple)
                    {
                        if (item.Answers == null)
                        {
                            if (item.YourAnswer != null)
                            {
                                answers.Add(new PerformerAnswer()
                                {
                                    AnswerId = -1,
                                    PerformerQuestion = new PerformerQuestion()
                                    {
                                        QuestionId = item.Id,
                                        QuestionName = item.QuestionText,
                                        YourAnswer = item.YourAnswer
                                    },
                                });
                            }
                        }
                        else
                        {
                            if (item.Answers.ToList().Any(y => y.Checked))
                            {
                                answers.AddRange(item.Answers.ToList()
                                    .Where(y => y.Checked)
                                    .Select(x => new PerformerAnswer()
                                    {
                                        PerformerQuestion = new PerformerQuestion
                                        {
                                            QuestionId = item.Id,
                                            QuestionName = item.QuestionText,
                                            YourAnswer = item.YourAnswer
                                        },
                                        AnswerId = x.Id,
                                        AnswerText = x.AnswerText
                                    })
                                    .ToList<PerformerAnswer>());
                            }
                            else
                            {
                                answers.Add(new PerformerAnswer
                                {
                                    AnswerId = -1,
                                    PerformerQuestion = new PerformerQuestion
                                    {
                                        QuestionId = item.Id,
                                        QuestionName = item.QuestionText,
                                        YourAnswer = item.YourAnswer
                                    },
                                });
                            }
                        }
                    }
                    else
                        answers.AddRange(new List<PerformerAnswer>()
                        {
                            new PerformerAnswer()
                            {
                                PerformerQuestion = new PerformerQuestion()
                                {
                                    QuestionId = item.Id,
                                    QuestionName = item.QuestionText,
                                    YourAnswer = item.YourAnswer
                                },
                                AnswerText = item.SelectedAnswerText,
                                AnswerId = item.SelectedAnswerId
                            }
                        });
                }

                InsertNewQuizData("QuizResult", new QuizResult
                {
                    QuizId = GetSelectedAnswerText(quiz).Questions.FirstOrDefault().QuizId,
                    FinishDate = DateTime.Now,
                    PerformerName = UserName,
                    PerformerAnswer = answers
                });
            });
        }

        public void InsertNewPerformerData(Quiz quiz)
        {
            var answers = new List<PerformerAnswer>();
            foreach (var item in GetSelectedAnswerText(quiz).Questions)
            {
                if (item.QuestionType.TypeOfQuestion == TypeOfQuestion.Multiple)
                {
                    if (item.Answers ==null)
                    {
                        if (item.YourAnswer != null)
                        {
                            answers.Add(new PerformerAnswer()
                            {
                                AnswerId = -1,
                                PerformerQuestion = new PerformerQuestion() { QuestionId = item.Id, QuestionName = item.QuestionText, YourAnswer = item.YourAnswer },
                            });
                        }
                    }
                    else
                    {
                        if (item.Answers.ToList().Any(y => y.Checked))
                        {
                            answers.AddRange(item.Answers.ToList().Where(y => y.Checked).Select(x => new PerformerAnswer()
                            {
                                PerformerQuestion = new PerformerQuestion() { QuestionId = item.Id, QuestionName = item.QuestionText, YourAnswer = item.YourAnswer },
                                AnswerId = x.Id,
                                AnswerText = x.AnswerText
                            }).ToList<PerformerAnswer>());
                        }
                        else
                        {
                            answers.Add(new PerformerAnswer()
                            {
                                AnswerId = -1,
                                PerformerQuestion = new PerformerQuestion() { QuestionId = item.Id, QuestionName = item.QuestionText, YourAnswer = item.YourAnswer },
                            });
                        }
                    }
                }
                else
                    answers.AddRange(new List<PerformerAnswer> { new PerformerAnswer
                        {
                            PerformerQuestion = new PerformerQuestion()
                            { QuestionId = item.Id, QuestionName = item.QuestionText,YourAnswer = item.YourAnswer },
                            AnswerText = item.SelectedAnswerText,
                            AnswerId = item.SelectedAnswerId }
                    });
            }

            InsertNewQuizData("QuizResult", new QuizResult
            {
                QuizId = GetSelectedAnswerText(quiz).Questions.FirstOrDefault().QuizId,
                FinishDate = DateTime.Now,
                PerformerName = UserName,
                PerformerAnswer = answers
            });
        }


        public bool IsAlreadyPassed(string id, out DateTime? resolveDate)
        {
            var performerResult = ODatabase
                .Select("FinishDate")
                .From<QuizResult>()
                .Where("QuizId")
                .Equals(id)
                .And("PerformerName")
                .Equals(UserName)
                .ToList<QuizResult>()
                .FirstOrDefault();

            if (performerResult != null)
            {
                resolveDate = performerResult.FinishDate;
                return true;
            }
            resolveDate = null;
            return false;
        }


        private string InsertNewQuizData(string className, object obj)
        {
            var json = JsonConvert.SerializeObject(
                obj,
                new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd"},
                new StringEnumConverter()
            );
            return ODatabase.Insert().Into(className + " Content " + json).Run().ORID.RID;
        }

        public List<QuizResult> SelectResults(string id)
        {
            return
                GetFullNameByAccount(ODatabase.Select("PerformerName,PerformerQuestion,PerformerAnswer")
                    .From<QuizResult>()
                    .Where("QuizId")
                    .Equals('#' + id)
                    .ToList<QuizResult>());
        }

        private List<QuizResult> GetFullNameByAccount(IEnumerable<QuizResult> results)
        {
            var fullNameByAccount = results as IList<QuizResult> ?? results.ToList();
            try
            {
                using (var memberEntry = new DirectoryEntry("LDAP://nspk.ru", "notification", "@3%Rh&8("))
                {
                    foreach (var item in fullNameByAccount)
                    {
                        var ds = new DirectorySearcher(memberEntry)
                        {
                            Filter = "(&(objectCategory=User)(objectClass=person)(sAMAccountName=" + item.PerformerName.Split('\\')[1] + "))",
                            Asynchronous = true,
                            SizeLimit = 10,
                            PageSize = 10
                        };
                        var k = ds.FindOne().GetDirectoryEntry();
                        item.PerformerName =
                            k.Properties["sn"][0] + " " +
                            k.Properties["givenName"][0] + ";" + k.Properties["mail"][0];
                    }
                }
                return fullNameByAccount.ToList();
            }
            catch
            {
                return fullNameByAccount.ToList();
            }

        }


        public sealed override IEnumerable<Quiz> Select()
        {
            var quizesMainInfo = ODatabase.Select().From<Quiz>().ToList<QuizMainInfo>();
            var rids = quizesMainInfo.Select(x => x.ORID.RID).ToList();

            var listofQuiz = new List<Quiz>();

            for (var i = 0; i < rids.Count; i++) // - костылина
                listofQuiz.Add
                (
                    new Quiz
                    {
                        QuizMainInfo = quizesMainInfo[i],
                        Questions =
                            ODatabase.Select("Answers,QuestionType,QuestionText,Id,AllowYourAnswer,SelectedYourAnswer,YourAnswer,QuestionPictures")
                                .From<Question>()
                                .Where("QuizId")
                                .Equals(quizesMainInfo[i].ORID.RID).OrderBy("Id")
                                .ToList<Question>()
                    }
                );

            foreach (var quiz in listofQuiz)
            foreach (var question in quiz.Questions)
                question.QuizId = quiz.QuizMainInfo.ORID.RID;

            return listofQuiz.OrderByDescending(x => x.QuizMainInfo.CreationDate).ToList();
        }

        public override Quiz Select(string id)
        {
           throw new NotImplementedException();
        }

        public sealed override async Task<IEnumerable<Quiz>> SelectAsync()
        {
            return await Task.Run(() =>
            {
                var quizesMainInfo = ODatabase.Select().From<Quiz>().ToList<QuizMainInfo>();
                var rids = quizesMainInfo.Select(x => x.ORID.RID).ToList();

                var listofQuiz = new List<Quiz>();

                for (var i = 0; i < rids.Count; i++) // - костылина
                    listofQuiz.Add
                    (
                        new Quiz
                        {
                            QuizMainInfo = quizesMainInfo[i],
                            Questions =
                                ODatabase
                                    .Select(
                                        "Answers,QuestionType, QuestionPictures,QuestionText,Id,AllowYourAnswer,SelectedYourAnswer,YourAnswer")
                                    .From<Question>()
                                    .Where("QuizId")
                                    .Equals(quizesMainInfo[i].ORID.RID)
                                    .OrderBy("Id")
                                    .ToList<Question>()
                        }
                    );

                foreach (var quiz in listofQuiz)
                foreach (var question in quiz.Questions)
                    question.QuizId = quiz.QuizMainInfo.ORID.RID;

                return listofQuiz.OrderByDescending(x => x.QuizMainInfo.CreationDate).ToList();
            });
        }

        public sealed override void SendToArchive(string rid)
        {
            var orid = new ORID(rid);
            ODatabase.Update(orid).Set("State", "Finished").Run();
        }


        private void SavePictures()
        {
            
            foreach (var quizQuestion in Quiz.Questions)
            {
                var pictures = new List<QuestionPictures>();
                foreach (var picture in quizQuestion.QuestionPictures)
                {
                    var base64 = picture.QuestionPicture.Split(new char[] {';'}, 2);
                    var name = base64[0].Split(new char[] { ':' }, 2)[1];

                    var bytes = Convert.FromBase64String(base64[1].Split(',')[1]);

                    //  var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(base64[1]);
                    var guid = Guid.NewGuid();
                    var path = Directory.CreateDirectory(FilePath + guid);
                    path.Create();

                    File.WriteAllBytes(FilePath + guid + "\\" + name, bytes);
                    pictures.Add(new QuestionPictures
                    {
                        QuestionPicture = NetworkPath + guid + "/" + name
                    });
                }
                quizQuestion.QuestionPictures = pictures;
               
            }
        }

    }
}