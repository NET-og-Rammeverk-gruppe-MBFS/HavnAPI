using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;


public class Anchorage : ShipPlaces
{
    private Queue<Ship> ShipQueue { get; }
    public Anchorage(string Name, int Spaces) : base(Name, Spaces)
    {
        ShipQueue = new Queue<Ship>();
    }

    /// <summary>
    /// Flytter skipet fra ventelisten til ankerplassen
    /// <summary>
    /// <param name="id">Id til skipet som flyttes</param>
    public Ship MoveShipFromQueue(int id)
    {
        return ShipQueue.Dequeue();
    }

    /// <summary>
    /// Legger til et  skip til ankerplassen
    /// <summary>
    /// <param name="ship">Skipet som skal legges til</param>
    public void AddShipToQueue(Ship ship)
    {
        ShipQueue.Enqueue(ship);
    }
}