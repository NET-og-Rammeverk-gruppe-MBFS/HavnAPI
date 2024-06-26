﻿using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
    public class ShipSailingArgs: EventArgs
    {
        public Ship TheShip { get; set; }
        /// <summary>
        /// Event der skipen holder på å seile
        /// </summary>
        public ShipSailingArgs(Ship theShip)
        {
            TheShip = theShip;
        }
    }
}

