using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulation;
public class Harbour : IHarbour
{
	private string name { get; }
	public List<HistoryService> ShipHistory { get; private set; }
	public List<HistoryService> ContainerHistory { get; private set; }
	private List<ShipPlaces> ShipPlacesList;
	private List<Ship> ShipsList { get; }
	private Anchorage AnchorageHarbour;

	/// <summary>
	/// Konstruktøren for Harbour klassen
	/// </summary>
	/// <param name="ships"></param>
	/// <param name="shipPlaces"></param>
	/// <param name="name"></param>
	/// <param name="SpacesInAnchorage"></param>
	/// <exception cref="InvalidNameException">Navnet på havnet kan ikke være tomt</exception>
	/// <exception cref="InvalidSpacesException">Antall plasser må være større enn 0.</exception>
	public Harbour(List<Ship> ships, List<ShipPlaces> shipPlaces, String name, int SpacesInAnchorage)
	{
		if (string.IsNullOrEmpty(name))
		{
            throw new InvalidNameException("Name can't be null or empty");
        }

		if (SpacesInAnchorage <= 0)
		{
            throw new InvalidSpacesException("SpacesInAnchorage must be greater than 0");
        }

		ShipsList = new List<Ship>(ships);
		ShipPlacesList = new List<ShipPlaces>(shipPlaces);
		ShipHistory = new List<HistoryService>();
		ContainerHistory = new List<HistoryService>();
		AnchorageHarbour = new Anchorage(name, SpacesInAnchorage);
	}

	public Harbour(String name, int SpacesInAnchorage)
	{

		ShipsList = new List<Ship>();
		ShipPlacesList = new List<ShipPlaces>();
		ShipHistory = new List<HistoryService>();
		ContainerHistory = new List<HistoryService>();
		AnchorageHarbour = new Anchorage(name, SpacesInAnchorage);
	}


 
	public void RemoveShip(Ship ship)
	{
		ShipsList.Remove(ship);
	}

	public void RemoveAllShip()
	{
		ShipsList.Clear();
	}

	public void AddShipPlace(ShipPlaces shipPlace)
	{
		ShipPlacesList.Add(shipPlace);
	}

	public void AddAllShipPlaces(List<ShipPlaces> shipPlaces)
	{
		ShipPlacesList.AddRange(shipPlaces);
	}

	/// <summary>
	/// Metoden legger til et skip til listen
	/// </summary>
	/// <param name="ship"></param>
	public void AddShip(Ship ship)
	{
		ShipsList.Add(ship);
	}

	public void AddAllShips(List<Ship> Allships)
	{
		ShipsList.AddRange(Allships);
	}


   /// <summary>
   /// Metoden starter simulasjonen
   /// </summary>
   /// <param name="Start">Det er starts dato/tid til simulasjonen</param>
   /// <param name="end">Det er dato/tid der du vil at simulasjonen skal stoppe</param>
   /// <exception cref="InvalidDateTimeRangeException">Kastes hvis start date er større en End Date</exception>
	public void Run(DateTime start, DateTime end)
	{
		if (end <= start)
		{
            throw new InvalidDateTimeRangeException("End date must be greater than start date", nameof(end));
        }

		//Vi starter med å lage en timer
		DateTime currentTime = start;

		//Så starter simulasjonen ved bruk av while, der den vil kjøre til sluttdato-en
		while (currentTime < end || ShipsList.Count != 0)
		{
			//Begge for-loops under går gjennom alle ship og plassene
			foreach (ShipPlaces ShipPlace in ShipPlacesList)
			{
                MoveShipFromAnchorage(ShipPlace, currentTime);
                foreach (Ship ship in new List<Ship>(ShipsList))
				{
					//Før det så lager denne metoden Containers objekters til shipet basert på antall i konstruktøren
					ship.MakeContainers();

					//Her så vil de se om destinasjonen til skipet og plassen som den itererer
					if (ship.PlaceDestination.Id == ShipPlace.Id)
					{
						//Det skjekker om det er ledig plass i plasssen fra for loop-en
						if (ShipPlace.AvailableSpace)
						{
							ship.AddHistory(new HistoryService(ShipPlace.Name, currentTime.AddSeconds(60)));
							ShipPlace.AddShip(MoveShip(ship));
						}

						//Og hvis det ikke er noen ledig plasser for skipets destinasjon, så flytter vi skipet til en ankerplass
						else
						{
							//Siden vi har to forskjellige lister i en ankerplass (en kø for losseplass og en vanlig liste for kaiplass)
							//så må vi ha en if test som sjekker først hva slags klasse type plassen er.
							//Og deretter plassere de i listen/kø-en
							AddShipToAnchorage(ship, currentTime);
						}
					}
				}
				//Etter en flere iterasjoner, så antar vi at alle skipene har seilet samtidig. Da legger vi 60 minutter for hver gang flere skip
				//har nådd destinasjonen
				currentTime = currentTime.AddMinutes(60);

				//Her så starter vi losse-prossessen
				//If testen sjekker om losseplassen er full og at det er en losseplass før man starter prossessen
				if (ShipPlace is Unloadingspace)
				{
					//Her så legger vi til miutter basert på hvor mange Container objekter det er i en skip, hvor mange
					//skip det er i losseplassen, og hvor fort losse-prossessen er basert på bruker av API-et
					currentTime.AddMinutes(((Unloadingspace)ShipPlace).UnloadContainer(currentTime, end));

					//Etter at alle skipene i losseplassen er ferdig, så returnerer vi listen tilbake til havn klassen
					AddAllShips(((Unloadingspace)ShipPlace).ReturnRepeatingShips());

					//Vi antar at når skipene har blir returnert til havn klassen, så seiler de til Start-of-sea passage som tar 60 min
					currentTime = currentTime.AddMinutes(60);

				}
				if (ShipPlace is Dockspace)
				{
					((Dockspace)ShipPlace).ReturnRepeatingShips();
                    currentTime = currentTime.AddMinutes(60);
                }
			}
		}

		foreach (ShipPlaces shipPlaces in ShipPlacesList)
		{
			ShipsList.AddRange(shipPlaces.ReturnAllShips());
			if(shipPlaces is Unloadingspace)
			{
				foreach (Container container in ((Unloadingspace)shipPlaces).containerSaved)
				{
					ContainerHistory.AddRange(container.Histories);
				}
			}
		}
		foreach (Ship ship1 in ShipsList)
		{
			ShipHistory.AddRange(ship1.histories);
		}
	}

	/// <summary>
	/// Flytter ship, ved å fjerne og returnere shipet
	/// </summary>
	/// <param name="TheShip"></param>
	/// <returns></returns>
	private Ship MoveShip(Ship TheShip)
	{
		Ship Ship = TheShip;
		ShipsList.Remove(TheShip);
		return Ship;
	}


	/// <summary>
	/// Det blir brukt i simulasjonen for å legge et skip til et spesifikk plass
	/// </summary>
	/// <param name="ShipPlaceId"></param>
	/// <param name="ship"></param>
	private void AddToSpesificPlace(int ShipPlaceId, Ship ship)
	{
		foreach (ShipPlaces Shipplace in ShipPlacesList)
		{
			if(ShipPlaceId == Shipplace.Id)
			{
				Shipplace.AddShip(MoveShip(ship));
			}
		}
	}

	
	/// <summary>
	/// Det blir brukt i run metoden som legger et ship i ankerplass hvis destinasjonsplassen er full
	/// </summary>
	/// <param name="ship"></param>
	/// <param name="current"> det er tiden som kommer fra run metoden. Det blir brukt for å lagre historikk i et ship</param>
	private void AddShipToAnchorage(Ship ship, DateTime current)
	{
		DateTime CurrentDateTime = current;
		if (ship.PlaceDestination is Unloadingspace)
		{
			CurrentDateTime = CurrentDateTime.AddMinutes(30);
			ship.AddHistory(new HistoryService(AnchorageHarbour.Name, CurrentDateTime));
            AnchorageHarbour.AddShipToQueue(MoveShip(ship));
		}
		else if (ship.PlaceDestination is Dockspace)
		{
			CurrentDateTime = CurrentDateTime.AddMinutes(30);
			ship.AddHistory(new HistoryService(ship.PlaceDestination.Name, CurrentDateTime));
            AnchorageHarbour.AddShip(MoveShip(ship));
		}
	}


	/// <summary>
	/// Metoden henter ut et ship fra en ankerplassen og legger det til destinasjonen. Hvis det ikke er i ankerplassen, så legger det direkte til destinasjonen
	/// </summary>
	/// <param name="shipPlaces"></param>
	/// <param name="current">Det er tiden som kommer fra run metoden. Det blir brukt for å lagre historikk i et ship</param>
	private void MoveShipFromAnchorage(ShipPlaces shipPlaces, DateTime current)
	{
		DateTime currentDateTime = current;
		if (AnchorageHarbour.ShipQueue.Count != 0 && AnchorageHarbour.ShipQueue.Peek().PlaceDestination.Id == shipPlaces.Id && shipPlaces.AvailableSpace)
		{
			//Her så fjerne vi skipet fra ankerplassen ved bruk av MoveShipFromQueue metoden og
			//plasserer det til destinasjonen ved bruk AddSpesificPlace metoden
			currentDateTime.AddMinutes(30);
            AnchorageHarbour.ShipQueue.Peek().AddHistory(new HistoryService(shipPlaces.Name, currentDateTime));
			AddToSpesificPlace(shipPlaces.Id, AnchorageHarbour.MoveShipFromQueue());
		}

		//Og hvis skipet ikke skal til losseplass, så skal den til en kaiplass
		else if (AnchorageHarbour.Ships.Count != 0 && AnchorageHarbour.Ships.First().PlaceDestination.Id == shipPlaces.Id && shipPlaces.AvailableSpace)
		{
			//Her så fjerne vi skipet fra ankerplassen ved bruk av MoveShip metoden og
			//plasserer det til destinasjonen ved bruk AddSpesificPlace metoden
			currentDateTime.AddMinutes(30);
            AnchorageHarbour.Ships.First().AddHistory(new HistoryService(shipPlaces.Name, currentDateTime));
			AddToSpesificPlace(shipPlaces.Id, AnchorageHarbour.Ships.First());
		}
		}
}