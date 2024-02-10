using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Services
{
	public interface IHarbour
	{
		public void Run(DateTime Start, DateTime end);

        public void RemoveShip(Ship ship);

        public void RemoveAllShip();

        public void AddShipPlace(ShipPlaces shipPlace);

        public void AddAllShipPlaces(List<ShipPlaces> shipPlaces);

        public void AddShip(Ship ship);

        public void AddAllShips(List<Ship> Allships);
    }
}

