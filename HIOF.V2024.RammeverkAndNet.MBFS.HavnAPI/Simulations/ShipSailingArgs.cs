using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
	public class ShipSailingArgs: EventArgs
	{
        public Ship ship { get; set; }
        /// <summary>
        /// Event der skipen holder på å seile
        /// </summary>
        public ShipSailingArgs(Ship ship)
		{
            this.ship = ship;
		}
	}
}

