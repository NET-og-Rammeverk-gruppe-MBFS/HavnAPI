using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations;

public abstract class ShipPlaces
{
    private static int Next = 0;
	public int Id { get; private set; }
	public string Name { get; private set; }
	public int Spaces { get; private set; }
	public ShipType Type { get; private set; }
	internal List<Ship> Ships { get; }
	internal List<Ship> Finished { get; }

    public event EventHandler<DepartingHarbourArgs> DepartingHarbour;

    /// <summary>
	/// konstruktør for ShipPlaces
	/// </summary>
	/// <param name="ShipName"></param>
	/// <param name="ShipSpaces"></param>
	/// <param name="type"></param>
	/// <exception cref="InvalidNameException"></exception>
	/// <exception cref="InvalidSpacesException"></exception>
    public ShipPlaces(string ShipName, int ShipSpaces, ShipType type)
	{
		if (string.IsNullOrWhiteSpace(ShipName))
		{
			throw new InvalidNameException("ShipName cannot be empty");
		}

		if (ShipSpaces <= 0)
		{
			throw new InvalidSpacesException("ShipSpaces must be greater than 0");
		}

		Name = ShipName;
		Spaces = ShipSpaces;
		Type = type;
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
	if (ship.Repeat == true)
		Ships.Add(ship);
	else
		Finished.Add(ship);
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
			}
			RaiseDepartingHarbour(ship);
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
			return Spaces > Ships.Count;
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

