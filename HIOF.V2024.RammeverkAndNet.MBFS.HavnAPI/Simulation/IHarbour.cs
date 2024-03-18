using System;
using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulation
{
	public interface IHarbour
	{
		public void Run(DateTime Start, DateTime end);

        public void RemoveShip(Ship ship);

        public void RemoveAllShip();

        public void AddShipPlace(ShipPlaces shipPlace);

        public void AddAllShipPlaces(Collection<ShipPlaces> shipPlaces);

        public void AddShip(Ship ship);

        public void AddAllShips(Collection<Ship> Allships);
    }
}

