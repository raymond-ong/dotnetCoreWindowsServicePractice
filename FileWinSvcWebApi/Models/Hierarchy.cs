using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    // TODO: Obsolete this class.
    // Currently this class is only used for returning Dummy Data to Web Client
    public class Hierarchy
    {
        public string Name { get; set; }

        public string NodeType { get; set; }

        public string FullPath { get; set; }

        public string Category { get; set; }

        public List<Hierarchy> Children { get; set; }
    }
}
