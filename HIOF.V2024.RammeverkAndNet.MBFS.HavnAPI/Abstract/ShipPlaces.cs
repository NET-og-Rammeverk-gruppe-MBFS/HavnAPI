﻿using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract
{
	public abstract class ShipPlaces
	{
        private static int Next = 0;
        public int Id { get; }
        public string Name { get; private set; }
		protected int Spaces { get; set; }
		internal List<Ship> Ships { get; }
		

		public ShipPlaces(string ShipName, int ShipSpaces)
		{
			Name = ShipName;
			Spaces = ShipSpaces;
			Ships = new List<Ship>();
			Id = Interlocked.Increment(ref Next);
		}

        /// <summary>
        /// Metoden legger til ship i de plassene for å simulere at de ha nådd denne plassen
        /// </summary>
        /// <param name="ship"></param>
        public virtual void AddShip(Ship ship)
		{
			Ships.Add(ship);
		}

        /// <summary>
        /// En metode som flytter ship fra og til en annen plass under simuleringen.
        /// </summary>
        /// <param name="id"></param>
		/// <returns></returns>
        public virtual Ship MoveShip(int id)
		{
			Ship TheShip;
			foreach (var SpesificShip in Ships)
			{
				if(SpesificShip.Id == id)
				{
					TheShip = SpesificShip;
					Ships.Remove(SpesificShip);
					return TheShip;
				}
			}
			return null;
		}


        /// <summary>
        /// Denen metoden er for å se om det er ledig plasser
        /// </summary>
        /// <returns></returns>
        public virtual bool AvailableSpace
        {
            get
            {
                return Spaces > Ships.Count;
            }
        }




    }
}

