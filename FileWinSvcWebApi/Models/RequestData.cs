using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class RequestData
    {
        public string HierarchyPath { get; set; }

        public List<string> Dimensions { get; set; }

        // If Kpis is specified, ignore this
        public List<string> KpiGroups { get; set; }

        // Optional: If empty, just follow KpiGroups
        public List<KpiInfo> Kpis { get; set; }

        // For pagination
        public int startIndex { get; set; }

        public int size { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
