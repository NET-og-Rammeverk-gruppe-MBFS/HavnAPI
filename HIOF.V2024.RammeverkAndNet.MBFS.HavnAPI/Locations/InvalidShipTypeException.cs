using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    /// <summary>
    /// Kaster et unntak når skipstypen er ugyldig
    /// </summary>
    internal class InvalidShipTypeException : Exception
    {
        public InvalidShipTypeException(string message) : base(message)
        {
        }
    }
}
