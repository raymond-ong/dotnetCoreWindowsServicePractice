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
    public class LayoutController : ControllerBase
    {        
        [HttpGet]
        public ActionResult<IEnumerable<LayoutData>> Get()
        {
            AprSqlAccessor accessor = new AprSqlAccessor("localhost");
            return accessor.RetrieveLayouts();
        }

        [HttpPost]
        [EnableCors(origins: "http://localhost:3000,http://localhost:60000", headers: "*", methods: "*")]
        public ActionResult<LayoutData> Post(LayoutData layout)
        {
            //Console.WriteLine("{0}, {1}, {2}", layout.Name, layout.LastUpdateTime, layout.LayoutJson);

            AprSqlAccessor accessor = new AprSqlAccessor("localhost");
            accessor.SaveLayout(layout);

            return layout;
        }
    }
}
