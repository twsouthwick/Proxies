using Microsoft.AspNetCore.Mvc;
using ProxySample.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProxySample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<ValueModel>> GetAll()
        {
            var result = Enumerable.Range(1, 10).Select(Create);

            return new ActionResult<IEnumerable<ValueModel>>(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ValueModel> GetById(int id) => Create(id);

        private static ValueModel Create(int i)
        {
            return new ValueModel
            {
                Item1 = i.ToString(),
                Item2 = (i + 1).ToString()
            };
        }
    }
}
