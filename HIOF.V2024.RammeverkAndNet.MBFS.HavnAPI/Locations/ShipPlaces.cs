using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations;

public abstract class ShipPlaces
{
    private static int Next = 0;
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
	internal List<Ship> Ships { get; }
	internal List<Ship> Finished { get; }

    public event EventHandler<DepartingHarbourArgs> DepartingHarbour;

	/// <param name="name">Navnet til plassen</param>
    /// <param name="shipSpaces">Antall plasser i plassen</param>
    /// <param name="shipType">Type skip som er tillatt i plassen</param>
    /// <exception cref="InvalidNameException"> Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidAmountException">Error for hvis du legger til ugyldig antall plasser som f.eks -1</exception>
    internal ShipPlaces(string placeName, int shipSpaces, ShipType shipType)
	{
		if (string.IsNullOrWhiteSpace(placeName))
		{
			throw new InvalidNameException("name cannot be empty");
		}

		if (shipSpaces <= 0)
		{
			throw new InvalidAmountException("ShipSpaces must be greater than 0");
		}

		Name = placeName;
		Space = shipSpaces;
		ShipType = shipType;
		Ships = new List<Ship>();
		Finished = new List<Ship>();
		Id = Interlocked.Increment(ref Next);
	}

	/// <summary>
	/// Metoden legger til ship i de plassene for å simulere at de ha nådd denne plassen
	/// </summary>
	/// <param name="ship"></param>
	internal virtual void AddShip(Ship ship)
	{
		ship.CurrentLocation = Name;
		if (ship.Repeat == true)
		{
			ship.Status = Status.Available;
		}
		Ships.Add(ship);
	}

	/// <summary>
	/// En metode som flytter ship fra og til en annen plass under simuleringen.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
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

	/// <summary>
	/// Det fjerner alle shipene som har repeterende seilinger fra lista. Det blir brukt i simulasjonen.
	/// </summary>
	/// <returns></returns>
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


	/// <summary>
	/// Det fjerner alle shipene fra lista. Det blir brukt i simulasjonen.
	/// </summary>
	/// <returns></returns>
	internal virtual List<Ship> ReturnAllShips()
	{
		List<Ship> OldShips = new List<Ship>(Ships);
		OldShips.AddRange(Finished);
		Ships.Clear();
		Finished.Clear();
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
			return Space > Ships.Count;
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

