﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    /// <summary>
    /// Kaster et unntak når det er ugyldig antall dager
    /// </summary>
    public class InvalidDaysInStorageAmountException : Exception
    {
        public InvalidDaysInStorageAmountException(string message) : base(message)
        {
        }
    }
}