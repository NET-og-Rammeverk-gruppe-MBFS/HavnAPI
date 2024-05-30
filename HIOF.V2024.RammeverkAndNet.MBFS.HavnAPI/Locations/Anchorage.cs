namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;


public class Anchorage : ShipPlaces
{
    internal Queue<Ship> ShipQueue { get; }

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

    /// <summary>
    /// Flytter skipet fra ventelisten til ankerplassen
    /// <summary>
    /// <param name="id">Id til skipet som flyttes</param>
    internal Ship MoveShipFromQueue()
    {
        return ShipQueue.Dequeue();
    }

    /// <summary>
    /// Legger til et  skip til ankerplassen
    /// <summary>
    /// <param name="ship">Skipet som skal legges til</param>
    internal void AddShipToQueue(Ship ship)
    {
        ShipQueue.Enqueue(ship);
    }

    /// <summary>
    /// Metode som returnerer liste av alle skip som er i listen og køen
    /// </summary>
    /// <returns> Alle skip i List<Ship> liste ></returns>
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

    /// <summary>
    /// AvailableSpace må override siden Losseplass har med Kø liste
    /// </summary>
    internal override bool AvailableSpace
    {
        get
        {
            return Space > Ships.Count + ShipQueue.Count;
        }
    }
}