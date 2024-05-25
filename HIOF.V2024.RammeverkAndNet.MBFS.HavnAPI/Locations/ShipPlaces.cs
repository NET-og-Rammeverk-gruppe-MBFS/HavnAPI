using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations;

public abstract class ShipPlaces
{
    private static int next = 0;
	/// <summary>
	/// ID til plassen. ID er autogenerert
	/// </summary>
	public int id { get; private set; }
	/// <summary>
	/// Navnet til plassen
	/// </summary>
	public string name { get; private set; }
	/// <summary>
	/// Antall plasser i plassen
	/// </summary>
	public int space { get; private set; }
	/// <summary>
	/// Dette forteller hva slags type skip som er tillatt
	/// </summary>
	public ShipType shipType { get; private set; }
	internal List<Ship> ships { get; }
	internal List<Ship> finished { get; }

    public event EventHandler<DepartingHarbourArgs> DepartingHarbour;

	/// <param name="name">Navnet til plassen</param>
    /// <param name="shipSpaces">Antall plasser i plassen</param>
    /// <param name="shipType">Type skip som er tillatt i plassen</param>
    /// <exception cref="InvalidNameException"> Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidAmountException">Error for hvis du legger til ugyldig antall plasser som f.eks -1</exception>
    public ShipPlaces(string name, int shipSpaces, ShipType shipType)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new InvalidNameException("name cannot be empty");
		}

		if (shipSpaces <= 0)
		{
			throw new InvalidAmountException("ShipSpaces must be greater than 0");
		}

		this.name = name;
		space = shipSpaces;
		this.shipType = shipType;
		ships = new List<Ship>();
		finished = new List<Ship>();
		id = Interlocked.Increment(ref next);
	}

	/// <summary>
	/// Metoden legger til ship i de plassene for å simulere at de ha nådd denne plassen
	/// </summary>
	/// <param name="ship"></param>
	internal virtual void AddShip(Ship ship)
	{
		ship.currentLocation = name;
		if (ship.repeat == true)
		{
			ship.status = Status.Available;
		}
		ships.Add(ship);
	}

	/// <summary>
	/// En metode som flytter ship fra og til en annen plass under simuleringen.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	internal virtual Ship MoveShip(int id)
	{
		Ship TheShip;
		foreach (var SpesificShip in ships)
		{
			if (SpesificShip.id == id)
			{
				TheShip = SpesificShip;
				ships.Remove(SpesificShip);
				return TheShip;
			}
		}
		return null;
	}

	/// <summary>
	/// Det fjerner alle shipene som har repeterende seilinger fra lista. Det blir brukt i simulasjonen.
	/// </summary>
	/// <returns></returns>
	internal List<Ship> ReturnRepeatingShips()
	{
		List<Ship> OldShips = new List<Ship>();
		foreach (Ship ship in new List<Ship>(ships))
		{
			if (ship.repeat is true)
			{
				OldShips.Add(ship);
				ships.Remove(ship);
				RaiseDepartingHarbour(ship);
			}
		}
		return OldShips;
	}


	/// <summary>
	/// Det fjerner alle shipene fra lista. Det blir brukt i simulasjonen.
	/// </summary>
	/// <returns></returns>
	internal virtual List<Ship> ReturnAllShips()
	{
		List<Ship> OldShips = new List<Ship>(ships);
		OldShips.AddRange(finished);
		ships.Clear();
		finished.Clear();
		return OldShips;
	}

	/// <summary>
	/// Denen metoden er for å se om det er ledig plasser
	/// </summary>
	/// <returns></returns>
	internal virtual bool AvailableSpace
	{
		get
		{
			return space > ships.Count;
		}
	}

	/// <summary>
	/// Metoden som blir kalt når et skip forlater havnen
	/// </summary>
	/// <param name="ship"></param>
    private void RaiseDepartingHarbour(Ship ship)
    {
        DepartingHarbour?.Invoke(this, new DepartingHarbourArgs(ship));
    }

}

