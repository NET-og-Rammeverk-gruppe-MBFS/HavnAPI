using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    public class AGV
    {
        internal Container container { get; set; }
        internal Status status { get; set; }

        internal AGV ()
        {
            status = Status.Available;
        }
    }
}
