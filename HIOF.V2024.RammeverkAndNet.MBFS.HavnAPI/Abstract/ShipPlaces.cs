using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract
{
	public abstract class ShipPlaces
	{
		private string Name { get; set; }
		private int Spaces { get; set; }
		private List<Ship> Ships { get; }
		

		public ShipPlaces(string ShipName, int ShipSpaces)
		{
			Name = ShipName;
			Spaces = ShipSpaces;
		}

		public void AddShip(Ship ship)
		{
		}

		public Ship MoveShip(int id)
		{
			return null;
		}

		public bool AvailableSpace()
		{
			return false;
		}




	}
}

