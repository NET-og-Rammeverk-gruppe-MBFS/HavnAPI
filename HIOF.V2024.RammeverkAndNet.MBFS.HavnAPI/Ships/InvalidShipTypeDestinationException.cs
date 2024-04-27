using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships
{
    /// <summary>
    /// Kaster et unntak når destinasjonen sin shipstype er ugyldig
    /// </summary>
    public class InvalidShipTypeDestinationException : Exception
    {
        public InvalidShipTypeDestinationException(string message) : base(message)
        {
        }
    }
}
