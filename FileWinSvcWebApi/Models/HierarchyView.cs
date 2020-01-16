using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class HierarchyView
    {
        // For now, we put only 1 entry "Default" because we don't have multiple user support yet
        public string ViewName { get; set; }
        public string HierarchyJson { get; set; }
        public string NodeSettingsJson { get; set; }
    }
}
