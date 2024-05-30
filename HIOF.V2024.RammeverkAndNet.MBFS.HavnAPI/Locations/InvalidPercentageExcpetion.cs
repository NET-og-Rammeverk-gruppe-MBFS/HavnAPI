using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    /// <summary>
    /// Kaster et unntak når det er feil bruk av prosent
    /// </summary>
    public class InvalidPercentageExcpetion : Exception
    {
        public InvalidPercentageExcpetion() : base()
        {
        }
        
        public InvalidPercentageExcpetion(string message) : base(message)
        {
        }

        public InvalidPercentageExcpetion(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
