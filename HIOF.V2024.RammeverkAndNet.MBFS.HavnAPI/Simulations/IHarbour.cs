﻿using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
    public interface IHarbour
    {
        public void Run(DateTime start, DateTime end);

        public void RemoveShipPlace(ShipPlaces shipPlaces);

        public void RemoveAllShipPlaces();

        public void RemoveShip(Ship ship);

        public void RemoveAllShip();

        public void AddShipPlace(ShipPlaces shipPlace);

        public void AddAllShipPlaces(List<ShipPlaces> shipPlaces);

        public void AddShip(Ship ship);

        public void AddAllShips(List<Ship> Allships);
    }
}

