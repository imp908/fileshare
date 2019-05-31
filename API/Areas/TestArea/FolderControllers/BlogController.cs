using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using mvccoresb.Domain.Interfaces;

using mvccoresb.Domain.TestModels;

using Newtonsoft.Json;

namespace mvccoresb.Default.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        IRepository _repo;
        IUOWBlogging _uow;

        public BlogController(IUOWBlogging uow,IRepository repo)
        {
            _uow=uow;
            _repo=repo;
        }

        [HttpGet]
        public ActionResult<IList<BlogEF>> Get()
        {
            var items =_repo.SkipTake<BlogEF>(0,100);
            if(items==null)
            {

            }
            return Ok(items);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<BlogEF> Get(int Id)
        {
            var item =_uow.GetByIntId(Id);
            if(item==null)
            {

            }
            return Ok(item);
        }
        [HttpGet("GetString/{id}")]
        public string GetString(int Id)
        {
            var item =_uow.GetByIntId(Id);
            if(item==null)
            {
                return string.Empty;            
            }
            return JsonConvert.SerializeObject(item);
        }


        // POST api/values
        [HttpPost]
        public ActionResult<BlogEF> Post([FromBody] BlogEF value)
        {
            var result = _uow.AddBlog(value);
            return Ok(result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] IBlogEF value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
