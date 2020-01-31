using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Models
{
    public class TableProps
    {
        public string ColumnName { get; set; }
        public string Datatype { get; set; }
        /* Remove unnecessary props
        public uint MaxLength { get; set; }
        public uint Precision { get; set; }
        public uint Scale { get; set; }
        public bool Nullable { get; set; }
        public bool PrimaryKey { get; set; }
        */
    }
}
