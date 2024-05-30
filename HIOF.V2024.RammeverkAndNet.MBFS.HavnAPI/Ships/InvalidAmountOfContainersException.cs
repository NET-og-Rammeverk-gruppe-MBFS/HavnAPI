using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships
{
    /// <summary>
    /// Kaster et unntak når antall containere er 0 eller mindre
    /// </summary>
    public class InvalidAmountOfContainersException : ArgumentOutOfRangeException
    {
        public InvalidAmountOfContainersException() : base()
        {
        }
        
        public InvalidAmountOfContainersException(string message) : base(message)
        {
        }

        public InvalidAmountOfContainersException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
