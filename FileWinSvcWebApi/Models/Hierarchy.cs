using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class Hierarchy
    {
        public string Name { get; set; }

        public string NodeType { get; set; }

        public string FullPath { get; set; }

        public string Category { get; set; }

        public List<Hierarchy> Children { get; set; }
    }
}
