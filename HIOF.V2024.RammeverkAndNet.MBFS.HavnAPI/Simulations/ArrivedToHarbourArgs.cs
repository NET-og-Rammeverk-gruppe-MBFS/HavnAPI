using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
	public class ArrivedToHarbourArgs : EventArgs
	{
        public Ship ship { get; private set; }

        /// <summary>
        /// Når en skip ankommer havnen
        /// </summary>
        public ArrivedToHarbourArgs(Ship theship)
		{
            ship = theship;
		}
	}
}

