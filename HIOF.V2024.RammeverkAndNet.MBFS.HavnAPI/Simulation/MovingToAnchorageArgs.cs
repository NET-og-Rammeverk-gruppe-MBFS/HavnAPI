using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulation
{
	public class MovingToAnchorageArgs :EventArgs
	{
        public Ship ship { get; set; }


        /// <summary>
        /// Lar skip seile til ankerplassen dersom destinasjonen er full
        /// </summary>
        public MovingToAnchorageArgs(Ship ship)
		{
           this.ship = ship;
        }
	}
}

