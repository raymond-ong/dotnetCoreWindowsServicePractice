using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class KpiGroupInfo
    {
        public string GroupName { get; set; }
        public List<string> KpiList{ get; set; }
    }
}
