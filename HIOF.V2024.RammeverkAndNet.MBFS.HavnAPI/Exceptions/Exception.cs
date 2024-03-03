using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Exceptions
{
    public class InvalidShipNameException : ArgumentException
    {
        public InvalidShipNameException(string message) : base(message)
        {
        }
    }

    public class InvalidSpacesException : ArgumentOutOfRangeException
    {
        public InvalidSpacesException(string message) : base(message)
        {
        }
    }

    public class InvalidHarbourNameException : ArgumentException
    {
        public InvalidHarbourNameException(string message) : base(message)
        {
        }
    }

    public class  InvalidDateTimeRangeException : ArgumentException
    {
        public InvalidDateTimeRangeException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}
