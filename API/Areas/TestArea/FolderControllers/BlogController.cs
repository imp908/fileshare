using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using mvccoresb.Domain.Interfaces;

using mvccoresb.Domain.TestModels;

namespace mvccoresb.Default.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        IUOWBlogging _uow;

        public BlogController(IUOWBlogging uow)
        {
            _uow=uow;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "blog1", "blog2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<IBlog> Get(int Id)
        {
            var item =_uow.GetByIntId(Id);
            if(item==null)
            {

            }
            return Ok(item);
        }
       

        // POST api/values
        [HttpPost]
        public ActionResult<BlogEF> Post([FromBody] BlogEF value)
        {
            var result = _uow.Add(value);
            return Ok(result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] IBlog value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
