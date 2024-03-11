using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    /// <summary>
    /// Kaster et unntak når frekvensen er for lav
    /// </summary>
    public class InvalidFrequencyException : Exception
    {
        public InvalidFrequencyException(string message) : base(message)
        {
        }
    }
}
