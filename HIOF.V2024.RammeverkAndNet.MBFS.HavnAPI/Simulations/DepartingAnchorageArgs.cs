using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
	public class DepartingAnchorageArgs : EventArgs
	{
        public Ship ship { get; private set; }
        /// <summary>
        /// Når skipet forlater sitt destinasjon
        /// </summary>
        public DepartingAnchorageArgs(Ship theship)
		{
            ship = theship;
		}
	}
}

