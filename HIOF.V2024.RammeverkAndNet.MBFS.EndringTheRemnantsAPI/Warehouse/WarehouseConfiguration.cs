using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class WarehouseConfiguration
    {
        public bool IsCoolStorage { get; set; }
        public bool IsDryStorage { get; set; }
        public bool IsHazardous { get; set; }
        public int TerminalCapacity { get; set; }
    }
}
