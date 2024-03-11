using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulation
{
	public class ReachedDestinationArgs : EventArgs
	{
		public Ship ship { get; set; }
        /// <summary>
        /// Event der skipen har ankommet sitt destinasjon
        /// </summary>
        public ReachedDestinationArgs(Ship ship)
		{
			this.ship = ship;
		}
	}
}

