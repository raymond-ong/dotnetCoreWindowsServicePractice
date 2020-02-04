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
    public class HierarchyKpiController : ControllerBase
    {
        [HttpGet]
        //public ActionResult<IEnumerable<LayoutData>> Get()
        public ActionResult<Dictionary<string, Dictionary<string, List<string>>>> Get()
        {
            IsaeDwAccessor accessor = new IsaeDwAccessor("192.168.56.130\\ISAESQLSERVER");
            Dictionary<string, Dictionary<string, List<string>>> retDict = accessor.GetConsolidatedHierarchyKpi();
            return retDict;
        }
    }
}
