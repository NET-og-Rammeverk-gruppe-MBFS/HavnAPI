using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulation
{
	public class MidnightStatusUpdateArgs : EventArgs
	{
		public Ship ship { get; set; }
		public Container container { get; set; }


		/// <summary>
		/// Status for hver skip når det er midnatt
		/// </summary>
		public MidnightStatusUpdateArgs(Ship ship)
		{
			this.ship = ship;
		}
		public MidnightStatusUpdateArgs(Container container)
		{
            this.container = container;
        }
	}
}

