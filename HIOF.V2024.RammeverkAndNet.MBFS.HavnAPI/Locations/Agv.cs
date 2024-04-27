using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    public class AGV
    {
        internal Container container { get; set; }
        internal Status status { get; set; }

        /// <summary>
        /// For å lage AGV kjøretøy
        /// </summary>
        internal AGV ()
        {
            status = Status.Available;
        }
    }
}
