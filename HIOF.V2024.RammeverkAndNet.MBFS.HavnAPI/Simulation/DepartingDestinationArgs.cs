﻿using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulation
{
	public class DepartingDestinationArgs : EventArgs
	{
        public Ship ship { get; private set; }
        /// <summary>
        /// Når skipet forlater sitt destinasjon
        /// </summary>
        public DepartingDestinationArgs(Ship theship)
		{
            ship = theship;
		}
	}
}

