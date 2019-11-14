using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    // Flat class, for testing dc.js
    public class KpiData
    {
        public string TargetName { get; set; }
        public string FullPath { get; set; }
        public string DiagnosticName { get; set; }
        public string KpiName { get; set; }
        public float KpiValue { get; set; }
        public string KpiStatus { get; set; } // Good, bad, fair

    }
}
