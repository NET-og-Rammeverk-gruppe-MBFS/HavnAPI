namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

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
	public Dockspace(string name, int shipSpaces, ShipType shipType) : base(name, shipSpaces, shipType)
	{
	}
}
