namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Dockspace : ShipPlaces
{
	/// <summary>
    /// For å lage kaiplass
    /// </summary>
	/// <param name="name">Navnet til kaiplassen</param>
    /// <param name="shipSpaces">Antall plasser i kaiplassen</param>
    /// <param name="shipType">Type skip som er tillatt i kaiplassen</param>
    /// <exception cref="InvalidNameException"> Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidAmountException">Error for hvis du legger til ugyldig antall plasser som f.eks -1</exception>
	public Dockspace(string placeName, int shipSpaces, ShipType shipType) : base(placeName, shipSpaces, shipType)
	{
	}

    internal override void AddShip(Ship ship)
	{
		ship.CurrentLocation = Name;
		if (ship.Repeat == true)
		{
			ship.Status = Status.Available;
            Ships.Add(ship);
		}
        else
        {
            ship.Status = Status.Finished;
            Finished.Add(ship);
        }
	}

}
