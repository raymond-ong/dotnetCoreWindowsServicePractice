using Accessors;
using FileWinSvcWebApi.Models;
using FileWinSvcWebApi.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Controllers
{

    // This class is for getting the Hierarchy from the Data Warehouse
    // TODO: Maybe it's easier if we divide the data into 2 parts:
    // [a] Hierarchical structure, that will only contain the keys
    // [b] The "flat" hierarchy list, that will serve as lookup table

    // For the user settings, maybe we can divide. But for the masterdata, we can return the full hierarchy as-is.
    // For the user settings, we divide the data so that:
    // [a] It's easier to build the tree structure, while consuming less storage space
    // [b] Lookup won't take much time -- but how are we going to build the flat hierarchy if the master data is hierarchical?
    [Route("api/[controller]")]
    [ApiController]
    public class HierarchyController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Hierarchy> Get()
        {
            // Realistically speaking, most likely the data warehouse will only contain "flat" hierarchy instead of hierarchical structure
            // For now, we retain the hierarchical structure here
            Hierarchy h = new Hierarchy()
            {
                Name = "Plant",
                FullPath = "//Plant",
                NodeType = "Plant",
                Children = new List<Hierarchy>() {
                    new Hierarchy() { Name = "Area01", FullPath = "//Plant/Area01", NodeType = "Folder", Children = new List<Hierarchy>() {
                                            new Hierarchy() { Name = "FIC001", FullPath = "//Plant/Area01/FIC001", NodeType = "Target", Children = null, Category="LOOP" },
                                            new Hierarchy() { Name = "FIC002", FullPath = "//Plant/Area01/FIC002", NodeType = "Target", Children = null, Category="LOOP" },
                                            new Hierarchy() { Name = "FIC003", FullPath = "//Plant/Area01/FIC003", NodeType = "Target", Children = null, Category="LOOP" },
                                            new Hierarchy() { Name = "DEVICE001", FullPath = "//Plant/Area01/DEVICE001", NodeType = "Target", Children = null, Category="DEVICE" },
                                            new Hierarchy() { Name = "DEVICE002", FullPath = "//Plant/Area01/DEVICE002", NodeType = "Target", Children = null, Category="DEVICE" },
                                            new Hierarchy() { Name = "DEVICE003", FullPath = "//Plant/Area01/DEVICE003", NodeType = "Target", Children = null, Category="DEVICE" },
                                    }
                    },
                    new Hierarchy() { Name = "Area02", FullPath = "//Plant/Area02", NodeType = "Folder", Children = null },
                    new Hierarchy() { Name = "Area03", FullPath = "//Plant/Area03", NodeType = "Folder", Children = null },
                }
            };
            return h;
        }

        [HttpGet]
        [Route("conso")]
        // access via http://localhost:60000/api/Hierarchy/conso
        public ActionResult<IEnumerable<HierarchyBase>> GetConsolidated()
        {
            // Get the "flat" hierarchy from the database first, then call another function to hydrate the children 
            IsaeDwAccessor accessor = new IsaeDwAccessor("192.168.56.130");
            List<HierarchyBase> hierConsoList = accessor.GetConsolidatedHierarchy();
            HierarchyHelper.HydrateHierarchyChildren(hierConsoList);
            // return the root children only
            var rootChildrenQry = hierConsoList.Where(h => h.ParentId == null);

            return rootChildrenQry.ToList();
        }
    }


}
