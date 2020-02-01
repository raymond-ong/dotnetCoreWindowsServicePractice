using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class DataRequest
    {
        public string HierarchyPath { get; set; }
        public List<string> Dimensions { get; set; }
        public List<string> KpiGroups { get; set; }
        public List<string> Kpis { get; set; }
    }
}
