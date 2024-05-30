using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations;

public abstract class ShipPlaces
{
    /// <summary>
    /// ID til plassen. ID er autogenerert
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Navnet til plassen
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Antall plasser i plassen
    /// </summary>
    public int Space { get; private set; }
    /// <summary>
    /// Dette forteller hva slags type skip som er tillatt
    /// </summary>
    public ShipType ShipType { get; private set; }

    public event EventHandler<DepartingHarbourArgs> DepartingHarbour;

    private static int Next = 0;
    internal Collection<Ship> Ships { get; }
    internal Collection<Ship> Finished { get; }

    /// <param name="name">Navnet til plassen</param>
    /// <param name="shipSpaces">Antall plasser i plassen</param>
    /// <param name="shipType">Type skip som er tillatt i plassen</param>
    /// <exception cref="InvalidNameException"> Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidAmountException">Error for hvis du legger til ugyldig antall plasser som f.eks -1</exception>
    internal ShipPlaces(SimulationName placeName, int shipSpaces, ShipType shipType)
    {
        if (string.IsNullOrWhiteSpace(placeName.ToString()))
        {
            throw new InvalidNameException("name cannot be empty.");
        }

        if (shipSpaces <= 0)
        {
            throw new InvalidAmountException("ShipSpaces must be greater than 0.");
        }

        Name = placeName.ToString();
        Space = shipSpaces;
        ShipType = shipType;
        Ships = new Collection<Ship>();
        Finished = new Collection<Ship>();
        Id = Interlocked.Increment(ref Next);
    }

    internal virtual void AddShip(Ship ship)
    {
        ship.CurrentLocation = Name;
        if (ship.Repeat == true)
        {
            ship.Status = Status.Available;
        }
        Ships.Add(ship);
    }

    internal virtual Ship MoveShip(int id)
    {
        Ship TheShip;
        foreach (var SpesificShip in Ships)
        {
            if (SpesificShip.Id == id)
            {
                TheShip = SpesificShip;
                Ships.Remove(SpesificShip);
                return TheShip;
            }
        }
        return null;
    }

    internal List<Ship> ReturnRepeatingShips()
    {
        List<Ship> OldShips = new List<Ship>();
        foreach (Ship ship in new List<Ship>(Ships))
        {
            if (ship.Repeat is true)
            {
                OldShips.Add(ship);
                Ships.Remove(ship);
                RaiseDepartingHarbour(ship);
            }
        }
        return OldShips;
    }

    internal virtual List<Ship> ReturnAllShips()
    {
        List<Ship> OldShips = new List<Ship>(Ships);
        OldShips.AddRange(Finished);
        Ships.Clear();
        Finished.Clear();
        return OldShips;
    }

    internal virtual bool AvailableSpace
    {
        get
        {
            return Space > Ships.Count;
        }
    }

    private void RaiseDepartingHarbour(Ship ship)
    {
        DepartingHarbour?.Invoke(this, new DepartingHarbourArgs(ship));
    }
}

