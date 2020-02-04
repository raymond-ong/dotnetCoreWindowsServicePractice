using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class KpiData
    {
        public string KpiGroupName { get; set; }
        public string KpiName { get; set; }

        public int KpiStatusValue { get; set; } // e.g. 20, 40, 50
        public string KpiStatus { get; set; } // e.g. Good, Bad, Fair
        public double KpiValue { get; set; } // Numeric value, like 90%
    }
}
