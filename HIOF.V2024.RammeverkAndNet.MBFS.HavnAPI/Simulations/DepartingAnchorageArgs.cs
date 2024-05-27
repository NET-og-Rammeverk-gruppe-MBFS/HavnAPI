using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
	public class DepartingAnchorageArgs : EventArgs
	{
        public Ship TheShip { get; private set; }
        /// <summary>
        /// Når skipet forlater venteplassen
        /// </summary>
        public DepartingAnchorageArgs(Ship theship)
		{
            TheShip = theship;
		}
	}
}

