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
    public class KpiController : ControllerBase
    {

        private string[] LoopKpis = new string[] { "Time in Control", "Time in Preferred Mode", "Tme MV Out of Limits", "Time in Alarm Status", "Time in Alarm Off Status" };
        private Dictionary<string, string[]> ValveKpis = new Dictionary<string, string[]>()
        {
            { "Valve Controllability", new string[] { "Time in Control", "Total Deviation Time" } },
            { "Valve Packing", new string[] { "Total Bad Packing Time" } },
            { "Valve Hunting", new string[] { "Total Hunting Time" } },
            { "Valve Inadequate Air", new string[] { "Total Inadequate Air Time" } },
            { "Valve Linkage", new string[] { "Total Bad Linkage Time" } },
            { "Valve Stiction", new string[] { "Total Stiction Time" } },
        };

        public enum KpiStatus
        {
            BAD = 50,
            FAIR = 40,
            GOOD = 20,
            UNCERTAIN = 10
        }
        private KpiStatus GenerateStatus(int iter)
        {
            if (iter % 10 == 0) return KpiStatus.GOOD;
            else if (iter % 10 == 1) return KpiStatus.BAD;
            else if (iter % 10 == 2) return KpiStatus.FAIR;
            else if (iter % 10 == 3) return KpiStatus.UNCERTAIN;
            else return KpiStatus.GOOD;
        }

        private float GenerateKpiValue(int iter)
        {
            if (iter % 10 == 0) return 80;
            else if (iter % 10 == 1) return 20 - iter % 20;
            else if (iter % 10 == 2) return 20 + iter % 60;
            else if (iter % 10 == 3) return 0;
            else return 80 + iter % 20;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KpiDataDcTest>> Get()
        {
            List<KpiDataDcTest> retList = new List<KpiDataDcTest>();
            var valveKpisFlat = ValveKpis.SelectMany(d => d.Value.Select(v => new Tuple<string, string>(d.Key, v)));

            // Loops
            for (int iArea = 0; iArea < 10; iArea++)
            {
                // Loops
                for (int iTarget = 0; iTarget < 10 + iArea*100; iTarget++)
                {
                    string targetName = string.Format("XFIC_{0:D2}_{1:D3}", iArea, iTarget);
                    KpiDataDcTest newData = new KpiDataDcTest()
                    {
                        TargetName = targetName,
                        DiagnosticName = "Loop Controllability",
                        KpiName = LoopKpis[iTarget % LoopKpis.Length],
                        FullPath = string.Format("//PLANT/Area_{0:D2}/{1}", iArea, targetName),
                        KpiStatus = GenerateStatus(iTarget).ToString(),                        
                        KpiValue = GenerateKpiValue(iTarget)
                    };

                    retList.Add(newData);
                }
                //Valves
                for (int iTarget = 0; iTarget < 10; iTarget++)
                {
                    string targetName = string.Format("DEVICE_{0:D2}_{1:D3}", iArea, iTarget);

                    foreach(var kpi in valveKpisFlat)
                    {
                        KpiDataDcTest newData = new KpiDataDcTest()
                        {
                            TargetName = targetName,
                            DiagnosticName = kpi.Item1,
                            KpiName = kpi.Item2,
                            FullPath = string.Format("//PLANT/Area_{0:D2}/{1}", iArea, targetName),
                            KpiStatus = GenerateStatus(iTarget).ToString(),
                            KpiValue = GenerateKpiValue(iTarget)
                        };
                        retList.Add(newData);
                    }
                }
            }


            return retList;
        }
    }
}
