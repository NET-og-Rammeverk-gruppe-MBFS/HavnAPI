using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships
{
    /// <summary>
    /// Kaster et unntak når destinasjonen er null
    /// </summary>
    public class InvalidDestinationException : ArgumentNullException
    {
        public InvalidDestinationException(string message) : base(message)
        {

        }
    }
}
