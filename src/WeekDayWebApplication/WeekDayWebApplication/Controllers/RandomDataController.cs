using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeekDayWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomDataController : ControllerBase
    {
        // GET: api/<RandomDataController>
        [HttpGet]
        public IEnumerable<object> Get()
        {
            return Get(10);
        }

        // GET api/<RandomDataController>/5
        [HttpGet("{count}")]
        public IEnumerable<object> Get(int count)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            List<object> ret = new List<object>();
            for(int i=0;i<count;i++)
            {
                object item = new {
                    Field1 = DateTime.Now,
                    Field2 = random.Next(0, 1000),
                    Field3 = $"item{i}",
                    Field4 = random.Next(0, 1000) 
                };
                ret.Add(item);
            }

            return ret;
        }
    }
}
