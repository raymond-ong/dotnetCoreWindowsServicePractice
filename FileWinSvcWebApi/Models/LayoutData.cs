using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class LayoutData
    {
        public string Name { get; set; }
        
        public string LayoutJson { get; set; }

        public DateTime LastUpdateTime { get; set; }

        //public int Revision { get; set; }

    }
}
