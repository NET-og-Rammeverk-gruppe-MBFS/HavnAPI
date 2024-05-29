using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
    public class ReachedDestinationArgs : EventArgs
    {
        public Ship TheShip { get; set; }
        /// <summary>
        /// Event der skipen har ankommet sitt destinasjon
        /// </summary>
        public ReachedDestinationArgs(Ship theShip)
        {
            TheShip = theShip;
        }
    }
}

