using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : Controller
    {
         // GET api/values
        [HttpGet]
        public QuizItem Get()
        {
            return QuizUOW.QuizGenerate();
        }

        // GET api/values/5
        [HttpGet("{key}")]
        public IEnumerable<IQuizItem> Get(int key_)
        {
            IEnumerable<IQuizItem> ret=new List<IQuizItem>();
              ret=from s in QuizUOW.QuizGenerate().array where s._key==key_ select s ;            
            return ret;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] QuizItem value)
        {
            return Json(value);
        }
    }
}