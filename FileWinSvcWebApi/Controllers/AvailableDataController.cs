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
    public class AvailableDataController
    {
        [HttpGet]
        public ActionResult<IEnumerable<AvailableData>> Get()
        {
            List<AvailableData> retList = new List<AvailableData>()
            {
                new AvailableData()  {SelectionType="NodeType", NodeType="Plant", KpiNameList = new string[] { "Plant Overall Score", "Plant Overall Efficiency", "Top 10 Worst Actors", "Overall Score per Area" } },
                new AvailableData()  {SelectionType="NodeType", NodeType="Folder", KpiNameList = new string[] { "Area Overall Score", "Area Efficiency" } },

                new AvailableData()  {SelectionType="Category", Category="LOOP", KpiNameList = new string[] { 
                    "Time in Control", 
                    "Time in Preferred Mode", 
                    "Tme MV Out of Limits",
                    "Time in Alarm Status",
                    "Time in Alarm Off Status"} },

                new AvailableData()  {SelectionType="Category", Category="DEVICE", KpiNameList = new string[] { 
                    "Time in Control",
                    "Total Deviation Time",
                    "Total Bad Packing Time",
                    "Total Hunting Time",
                    "Total Inadequate Air Time",
                    "Total Bad Linkage Time",
                    "Total Stiction Time",
                } },


                new AvailableData()  {SelectionType="HierarchyName", Hierarchy="//Plant/Area01/DEVICE003", KpiNameList = new string[] { "Fault Monitoring" } }
            };


            return retList;
        }
    }
}
