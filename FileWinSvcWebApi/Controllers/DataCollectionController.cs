using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataCollectionController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "valueA", "valueB" };
        }
    }
}
