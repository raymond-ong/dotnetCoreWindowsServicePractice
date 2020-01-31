using Accessors;
using FileWinSvcWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace FileWinSvcWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KpiMasterController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Dictionary<string, List<string>>> Get()
        {
            // No need for now...maybe needed next time when querying KPI info like thresholds
            //IsaeDwAccessor accessor = new IsaeDwAccessor("localhost");
            //Dictionary<string, List<string>> retDict = accessor.GetM();
            //return retDict;
            throw new NotImplementedException();
        }
    }
}
