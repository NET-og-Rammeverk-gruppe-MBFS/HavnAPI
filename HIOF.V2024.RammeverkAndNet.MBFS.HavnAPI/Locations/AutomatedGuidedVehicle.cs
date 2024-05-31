using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    public class AutomatedGuidedVehicle
    {
        internal Container Container { get; set; }
        internal Status Status { get; set; }
        
        internal AutomatedGuidedVehicle ()
        {
            Status = Status.Available;
        }
    }
}
