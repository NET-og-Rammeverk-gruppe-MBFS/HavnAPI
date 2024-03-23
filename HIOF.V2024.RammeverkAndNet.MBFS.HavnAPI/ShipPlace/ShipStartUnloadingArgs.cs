using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
	public class ShipStartUnloadingArgs : EventArgs
	{
		public Ship ship { get; private set; }

		/// <summary>
		/// Når losseplassen begynner å losse containers
		/// </summary>
		public ShipStartUnloadingArgs(Ship theship)
		{
			ship = theship;
		}

	}
}

