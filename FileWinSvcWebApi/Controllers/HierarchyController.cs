﻿using FileWinSvcWebApi.Models;
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
    public class HierarchyController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Hierarchy> Get()
        {
            Hierarchy h = new Hierarchy()
            {
                Name = "Plant",
                FullPath = "//Plant",
                NodeType = "Plant",
                Children = new List<Hierarchy>() {
                    new Hierarchy() { Name = "Area01", FullPath = "//Plant/Area01", NodeType = "Plant", Children = new List<Hierarchy>() {
                                            new Hierarchy() { Name = "FIC001", FullPath = "//Plant/Area01/FIC001", NodeType = "Target", Children = null },
                                            new Hierarchy() { Name = "FIC002", FullPath = "//Plant/Area01/FIC002", NodeType = "Target", Children = null },
                                    }
                    },
                    new Hierarchy() { Name = "Area02", FullPath = "//Plant/Area02", NodeType = "Folder", Children = null },
                    new Hierarchy() { Name = "Area03", FullPath = "//Plant/Area03", NodeType = "Folder", Children = null },
                }
            };
            return h;
        }
    }
}
