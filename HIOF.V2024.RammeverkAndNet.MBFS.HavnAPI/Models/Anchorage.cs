using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;


public class Anchorage : ShipPlaces
{
    internal Queue<Ship> ShipQueue { get; }
    public Anchorage(string Name, int Spaces) : base(Name, Spaces)
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

    internal override bool AvailableSpace
    {
        get
        {
            return Spaces > Ships.Count+ShipQueue.Count;
        }
    }
}