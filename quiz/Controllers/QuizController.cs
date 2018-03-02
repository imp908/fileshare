using System.Linq;
using System;
using System.CodeDom;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Intranet.Models;
using Intranet.Models.QuizModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Intranet.Controllers
{
    public class QuizController : Controller
    {
        private readonly IntranetHelper<Quiz> Quizhelper;

        public QuizController(IntranetHelper<Quiz> quizhelper)
        {
            Quizhelper = quizhelper;
        }   

        //*** Конструктор опросов
        [HttpPost]
        public ActionResult Constructor(string output)
        {
            var quizhelper = new QuizHelper(JsonConvert.DeserializeObject<Quiz>(output), new OrientDbContext());
            if (!quizhelper.IsEmployeInGroup()) return Redirect(Messages.AccessError, MessageType.Error);
            
            quizhelper.Insert();
            return View();
        }


        [HttpGet]
        public ActionResult Constructor(string id, string user)
        {
            if (!Quizhelper.IsEmployeInGroup()) return Redirect(Messages.AccessError, MessageType.Error);
            var executionQuiz = id != null? Quizhelper.Select().FirstOrDefault(s=>s.QuizMainInfo.ORID.RID.Remove(0,1).Equals(id)) : null;

            ViewBag.Quiz = executionQuiz != null ? JsonConvert.SerializeObject(executionQuiz) : "{}";
            return View();
        }


        //*** Прохождение опроса
        [HttpPost]
        public ActionResult Execute(Quiz quiz)
        {
           
            #region errors
            //var p = ModelState
            //.Where(x => x.Value.Errors.Count > 0)
            //.Select(x => new { x.Key, x.Value.Errors })
            //.ToArray();
            //quizhelper = new QuizHelper(model, _dataBaseContext.GetDataBase());
            #endregion

            if (!ModelState.IsValid)
            {
                quiz.Questions = quiz.Questions.OrderByDescending(x => x.Id).ToList();
                return View(quiz);
            }
            DateTime? executedDate;
            if (((IQuiz)Quizhelper).IsAlreadyPassed(quiz.QuizMainInfo.ORID.RID, out executedDate))
            {
                return Redirect(Messages.AlreadyPassed + executedDate.Value.ToString("dd.MM.yyyy"), MessageType.Info);
            }
            ((IQuiz)Quizhelper).InsertNewPerformerData(quiz);
            return Redirect(Messages.Success, MessageType.Success);
        }


        [HttpGet]
        public ActionResult Execute(string id)
        {
          try
            {
                if (id.Equals("20:59")) return Redirect("http://my.nspk.ru/Quiz/Execute/?id=20:60"); //
            }
            catch (Exception e)
            {
                return Redirect(Messages.PageError, MessageType.Error);
            }


            var executedDate = new DateTime?();

            //if (((IQuiz)Quizhelper).IsAlreadyPassed(id,out executedDate))
            //{
            //    return Redirect(Messages.AlreadyPassed + executedDate.Value.ToShortDateString(),MessageType.Info);
            //}

            var executionQuiz = (Quizhelper.Select()).FirstOrDefault(y => y.QuizMainInfo.ORID.RID.Equals("#" + id));
            if (executionQuiz != null)
            {
                if (executionQuiz.QuizMainInfo.State == State.Drafted && !Quizhelper.IsEmployeInGroup())
                {
                    return Redirect(Messages.NotPublished, MessageType.Error);
                }

                if (executionQuiz.QuizMainInfo.State == State.Finished)
                {
                    return Redirect(Messages.FinishQuiz, MessageType.Error);
                }

                executionQuiz.Questions = executionQuiz.Questions.OrderByDescending(x => x.Id).ToList();
                //executionQuiz = executionQuiz.GetPictures();
            }
            else
            {
                return Redirect(Messages.PageError, MessageType.Error);
            }


            return View(executionQuiz);
        }


        private ActionResult Redirect(string str,MessageType type)
        {
            return RedirectToAction("InfoPage", "Quiz", new InfoMessage { MessageText = str, MessageType = type });
        }


        //*** Список опросов
        public async Task<ActionResult> List()
        {
            if (!Quizhelper.IsEmployeInGroup()) return Redirect(Messages.AccessError, MessageType.Error);

            var quizes = await Quizhelper.SelectAsync();
            return View(quizes.Where(x => x.QuizMainInfo.State != State.Finished).ToList());
        }


        public ActionResult InfoPage(InfoMessage message)
        {
            return View(message);
        }


        [HttpGet]
        //*** Список результатов
        public async Task<ActionResult> Results(string id)
        {
            if (id == null)
            {
                return Redirect(Messages.PageError, MessageType.Error);
            }

            if (!Quizhelper.IsEmployeInGroup()) return Redirect(Messages.AccessError, MessageType.Error);
          
            ViewBag.Quiz = (await Quizhelper.SelectAsync()).FirstOrDefault(y => y.QuizMainInfo.ORID.RID.Equals("#" + id));
            return ViewBag.Quiz == null ? Redirect(Messages.PageError, MessageType.Error) : View(((IQuiz)Quizhelper).SelectResults(id));
        }


        //*** Список архивных опросов
        public ActionResult Archive()
        {
            return !Quizhelper.IsEmployeInGroup() ? 
                Redirect(Messages.AccessError, MessageType.Error) : 
                View(Quizhelper.Select().Where(x=>x.QuizMainInfo.State == State.Finished).ToList());
        }


        //*** Отправить опрос в архив
        [HttpPost]
        public async Task<ActionResult> List(string rid)
        {           
            if (!Quizhelper.IsEmployeInGroup()) return Redirect(Messages.AccessError, MessageType.Error);

            Quizhelper.SendToArchive(rid);

            return View((await Quizhelper.SelectAsync()).Where(x => x.QuizMainInfo.State != State.Finished).ToList());
        }

    }
}