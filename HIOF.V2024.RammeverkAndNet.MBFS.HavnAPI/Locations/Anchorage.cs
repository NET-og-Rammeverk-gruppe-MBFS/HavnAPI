namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;


public class Anchorage : ShipPlaces
{
    /// <summary>
    /// For å lage venteplass
    /// </summary>
    /// <param name="name">Navnet til venteplassen</param>
    /// <param name="shipSpaces">Antall plasser i venteplassen</param>
    /// <param name="shipType">Type skip som er tillatt i venteplassen</param>
    /// <exception cref="InvalidNameException"> Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidAmountException">Error for hvis du legger til ugyldig antall plasser som f.eks -1</exception>
    internal Anchorage(SimulationName name, int shipSpaces, ShipType shipType) : base(name, shipSpaces, shipType)
    {
        ShipQueue = new Queue<Ship>();
    }


    internal Queue<Ship> ShipQueue { get; }
    internal Ship MoveShipFromQueue()
    {
        return ShipQueue.Dequeue();
    }


    internal void AddShipToQueue(Ship ship)
    {
        ShipQueue.Enqueue(ship);
    }

    internal override List<Ship> ReturnAllShips()
    {
        List<Ship> OldShips = new List<Ship>(Ships);
        foreach (var ship in Ships)
        {
            OldShips.Add(ship);
        }
        foreach (var shipInQueue in ShipQueue)
        {
            OldShips.Add(shipInQueue);
        }
        Ships.Clear();
        ShipQueue.Clear();
        return OldShips;
    }

    internal override bool AvailableSpace
    {
        get
        {
            return Space > Ships.Count + ShipQueue.Count;
        }
    }
}