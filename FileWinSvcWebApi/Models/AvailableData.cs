using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class AvailableData
    {
        // either "Hierarchy", "Category", "NodeType"
        public string SelectionType { get; set; }
        public string Hierarchy { get; set; }
        public string Category { get; set; }
        public string NodeType { get; set; }

        public string[] KpiNameList { get; set; }
    }
}
