using Accessors;
using FileWinSvcWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace FileWinSvcWebApi.Controllers
{
    // This class is for managing the hierarchy and its associated pages
    // Dilemma: Are we going to store everything in the hierarchy+user changes, or are we just gonna store the delta made by the user?
    // Plan A: 
    // [1] Table1: We store the entire hierarchy as JSON, but only include the keys    
    // [2] Table2: The User settings, will be stored in a flat structure. FK: key from Table1

    // [Future Consideration]: Maybe there will be a requirement to have different "Views" for different users
    // e.g. [Web View for User1, Report View for User1, Web View for User2, Report View for User2] or 
    //      [Web + Report View for User1]

    // [Negative Scenario 1]
    // - User made changes to the hierarchy
    // - The following day, the Data Warehouse hierarchy changes. Some of the nodes configed by the user is no longer there.
    // - We don't show these nodes anymore? Or we still show this, but this node will contain outdated data only (ui also shows some warning that this ndoe has been deleted)

    [Route("api/[controller]")]
    [ApiController]
    public class HierarchyViewsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<HierarchyView>> Get()
        {
            AprSqlAccessor accessor = new AprSqlAccessor("localhost");
            return accessor.RetrieveHierarchyViews();
        }

        [HttpPost]
        //[EnableCors(origins: "http://localhost:3000,http://localhost:60000", headers: "*", methods: "*")]
        public ActionResult<HierarchyView> Post(HierarchyView viewData)
        {
            //Console.WriteLine("{0}, {1}, {2}", layout.Name, layout.LastUpdateTime, layout.LayoutJson);
            try
            {
                AprSqlAccessor accessor = new AprSqlAccessor("localhost");
                accessor.SaveHierarchyView(viewData);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return viewData;
        }
    }
}
