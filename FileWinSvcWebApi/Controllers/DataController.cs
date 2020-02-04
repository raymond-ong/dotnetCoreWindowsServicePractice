using Accessors;
using FileWinSvcWebApi.Models;
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
    public class DataController : ControllerBase
    {
        [HttpPost]
        public ActionResult<IEnumerable<ResultData>> Post(RequestData request)
        {
            IsaeDwAccessor accessor = new IsaeDwAccessor("192.168.56.130\\ISAESQLSERVER");
            Console.WriteLine("Post");
            List<ResultData> retData = accessor.queryData(request);

            return null;
        }
    }
}
