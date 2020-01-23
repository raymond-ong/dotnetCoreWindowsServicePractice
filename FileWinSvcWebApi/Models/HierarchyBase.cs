using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    // Basic Hierarchy Properties
    public class HierarchyBase
    {
        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string ServerName { get; set; }

        public string NodeName { get; set; }

        public string FullPath { get; set; }

        public string UnitType { get; set; }

        public List<HierarchyBase> Children { get; set; }
    }
}
