using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
	public class ShipUnloadedArgs : EventArgs
	{
        public Ship ship { get; private set; }
        /// <summary>
        /// Event når skipen blir tømt
        /// </summary>
        public ShipUnloadedArgs(Ship theship)
		{
            ship = theship;
		}
	}
}

