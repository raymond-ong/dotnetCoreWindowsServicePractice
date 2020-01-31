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
    public class DimensionsController : ControllerBase
    {
        [HttpGet]
        //public ActionResult<IEnumerable<LayoutData>> Get()
        public ActionResult<IEnumerable<TableProps>> Get()
        {
            IsaeDwAccessor accessor = new IsaeDwAccessor("localhost");
            List<TableProps> retList = accessor.GetDimensions();
            return retList;
        }
    }
}
