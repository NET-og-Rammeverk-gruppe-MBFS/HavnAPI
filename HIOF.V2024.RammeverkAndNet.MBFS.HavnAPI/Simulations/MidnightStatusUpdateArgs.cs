using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
	public class MidnightStatusUpdateArgs : EventArgs
	{
		public Ship ship { get; private set; }


		/// <summary>
		/// Status for hver skip når det er midnatt
		/// </summary>
		public MidnightStatusUpdateArgs(Ship theship)
		{
			ship = theship;
		}
	}
}

