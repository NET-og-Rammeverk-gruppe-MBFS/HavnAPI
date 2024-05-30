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
    public class InvalidShipTypeException : Exception
    {
        public InvalidShipTypeException() : base()
        {
        }
        
        public InvalidShipTypeException(string message) : base(message)
        {
        }

        public InvalidShipTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
