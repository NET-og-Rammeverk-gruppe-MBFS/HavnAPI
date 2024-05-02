using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;
using System.Collections.ObjectModel;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations;
public class Harbour : IHarbour
{
	private string Name { get; }
	/// <summary>
	/// Liste over alle loggførte plassering for hver skip
	/// </summary>
	public List<HistoryService> ShipHistory { get; private set; }
	/// <summary>
	/// Liste over alle loggførte plassering for hver container
	/// </summary>
	public List<HistoryService> ContainerHistory { get; private set; }
	/// <summary>
	/// Liste over alle plasser i havnen
	/// </summary>
	public List<ShipPlaces> ShipPlacesList;
	/// <summary>
	/// Liste over alle skip i havnen
	/// </summary>
	public List<Ship> ShipsList { get; }
	private Anchorage AnchorageHarbour;

	public event EventHandler<ArrivedToHarbourArgs> ArrivedToHarbour;
	public event EventHandler<DepartingAnchorageArgs> DepartingAnchorage;
    public event EventHandler<MidnightStatusUpdateArgs> MidnightStatusUpdate;
	public event EventHandler<MovingToAnchorageArgs> MovingToAnchorage;
	public event EventHandler<ReachedDestinationArgs> ReachedDestination;
	public event EventHandler<ShipSailingArgs> ShipSailing;

    /// <summary>
    /// For å lage havn objekt
    /// </summary>
    /// <param name="ships"> Liste med Ship objekter som blir lagt til denne objektet.</param>
    /// <param name="shipPlaces">Liste med objekter som Unloadingspace og Dockspace. OBS når du skal lage en liste, så må du bruke ShipPlace som datatype</param>
    /// <param name="name">Navnet til havnen</param>
    /// <param name="SpacesInAnchorage">Antall plasser i venteplassen</param>
    /// <exception cref="InvalidNameException">Navnet på havnet kan ikke være tomt</exception>
    /// <exception cref="InvalidSpacesException">Antall plasser må være større enn 0.</exception>
    public Harbour(string name, int SpacesInAnchorage, List<Ship> ships, List<ShipPlaces> shipPlaces)
	{
		if (string.IsNullOrEmpty(name))
		{
            throw new InvalidNameException("Name can't be null or empty");
        }

		if (SpacesInAnchorage <= 0)
		{
            throw new InvalidAmountException("SpacesInAnchorage must be greater than 0");
        }

		ShipsList = new List<Ship>(ships);
		ShipPlacesList = new List<ShipPlaces>(shipPlaces);
		ShipHistory = new List<HistoryService>();
		ContainerHistory = new List<HistoryService>();
		AnchorageHarbour = new Anchorage(name + " venteplass", SpacesInAnchorage, ShipType.All);
		Name = name;
	}

	/// <summary>
    /// For å lage havn objekt
    /// </summary>
    /// <param name="name">Navnet til havnen</param>
    /// <param name="SpacesInAnchorage">Antall plasser i venteplassen</param>
    /// <exception cref="InvalidNameException">Navnet på havnet kan ikke være tomt</exception>
    /// <exception cref="InvalidSpacesException">Antall plasser må være større enn 0.</exception>
	public Harbour(string name, int SpacesInAnchorage)
	{
		if (string.IsNullOrEmpty(name))
		{
            throw new InvalidNameException("Name can't be null or empty");
        }

		if (SpacesInAnchorage <= 0)
		{
            throw new InvalidAmountException("SpacesInAnchorage must be greater than 0");
        }

		ShipsList = new List<Ship>();
		ShipPlacesList = new List<ShipPlaces>();
		ShipHistory = new List<HistoryService>();
		ContainerHistory = new List<HistoryService>();
		AnchorageHarbour = new Anchorage(name + " venteplass", SpacesInAnchorage, ShipType.All);
		Name = name;
	}

	/// <summary>
	/// Metode for å fjerne et skip
	/// </summary>
	/// <param name="ship">skipet som skal bli fjernet</param>
	public void RemoveShip(Ship ship)
	{
		ShipsList.Remove(ship);
	}

	/// <summary>
	/// Metode for å fjerne alle skipene fra havn objektet
	/// </summary>
	public void RemoveAllShip()
	{
		ShipsList.Clear();
	}

	/// <summary>
	/// Metode for å legge til Unlaodingspace eller Dockspace
	/// </summary>
	/// <param name="shipPlace">Unloadingspace eller Dockspace objekt</param>
	public void AddShipPlace(ShipPlaces shipPlace)
	{
		ShipPlacesList.Add(shipPlace);
	}

	/// <summary>
	/// Metode for å legge til alle objektene som arver ShipPlace som Unloadingspace og Dockspace
	/// </summary>
	/// <param name="shipPlaces">Liste med Unloadingspace og/eller Dockspace objekter</param>
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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="allships"></param>
	public void AddAllShips(List<Ship> allships)
	{
		ShipsList.AddRange(allships);
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
		while (currentTime < end && (ShipsList.Count + AnchorageHarbour.Ships.Count + AnchorageHarbour.ShipQueue.Count) != 0)
		{
			//Begge for-loops under går gjennom alle ship og plassene
			foreach (ShipPlaces ShipPlace in ShipPlacesList)
			{
                MoveShipFromAnchorage(ShipPlace, currentTime);
                foreach (Ship ship in new List<Ship>(ShipsList))
				{
					//Her så vil de se om destienasjonen til skipet og plassen som den itererer
					if (ship.PlaceDestination.Id == ShipPlace.Id)
					{
						//Før det så lager denne metoden Containers objekters til shipet basert på antall i konstruktøren
                    	ship.MakeContainers();
                        RaiseArrivedToHarbour(ship);
                        RaiseShipSailing(ship);
                        //Det skjekker om det er ledig plass i plasssen fra for loop-en
                        if (ShipPlace.AvailableSpace)
						{
							ship.AddHistory(new HistoryService(ship.ShipName, currentTime.AddSeconds(60), ShipPlace.Name));
							RaiseReachedDestination(ship);
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
                    if (currentTime.Hour == 0)
					{
						RaiseMidnightStatusUpdate(ship);
					}
                        
                }
				//Her så starter vi losse-prossessen
				//If testen sjekker om losseplassen er full og at det er en losseplass før man starter prossessen
				if (ShipPlace is Unloadingspace)
				{
					//Her så legger vi til miutter basert på hvor mange Container objekter det er i en skip, hvor mange
					//skip det er i losseplassen, og hvor fort losse-prossessen er basert på bruker av API-et
					currentTime = currentTime.AddMinutes(((Unloadingspace)ShipPlace).UnloadContainer(currentTime, end));
					currentTime = currentTime.AddMinutes(((Unloadingspace)ShipPlace).TargetContainerSpace.OverdueContainers(currentTime, end));
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
            //Etter en flere iterasjoner, så antar vi at alle skipene har seilet samtidig. Da legger vi 60 minutter for hver gang flere skip
            //har nådd destinasjonen
            currentTime = currentTime.AddMinutes(60);
        }

		foreach (ShipPlaces shipPlaces in ShipPlacesList)
		{
			ShipsList.AddRange(shipPlaces.ReturnAllShips());
			if(shipPlaces is Unloadingspace)
			{
				ContainerHistory.AddRange(((Unloadingspace)shipPlaces).ContainerHistory);
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
	private Ship MoveShip(Ship theShip)
	{
		Ship Ship = theShip;
		ShipsList.Remove(theShip);
		return Ship;
	}


	/// <summary>
	/// Det blir brukt i simulasjonen for å legge et skip til et spesifikk plass
	/// </summary>
	/// <param name="ShipPlaceId"></param>
	/// <param name="ship"></param>
	private void AddToSpesificPlace(int shipPlaceId, Ship ship)
	{
		foreach (ShipPlaces Shipplace in ShipPlacesList)
		{
			if(shipPlaceId == Shipplace.Id)
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
			ship.AddHistory(new HistoryService(ship.ShipName, CurrentDateTime, AnchorageHarbour.Name));
			RaiseMovingToAnchorage(ship);
            AnchorageHarbour.AddShipToQueue(MoveShip(ship));
		}
		else if (ship.PlaceDestination is Dockspace)
		{
			CurrentDateTime = CurrentDateTime.AddMinutes(30);
			ship.AddHistory(new HistoryService(ship.ShipName, CurrentDateTime, AnchorageHarbour.Name));
            RaiseMovingToAnchorage(ship);
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
            AnchorageHarbour.ShipQueue.Peek().AddHistory(new HistoryService(AnchorageHarbour.ShipQueue.Peek().ShipName, currentDateTime, shipPlaces.Name));
			RaiseDepartingAnchorage(AnchorageHarbour.ShipQueue.Peek());
			AddToSpesificPlace(shipPlaces.Id, AnchorageHarbour.MoveShipFromQueue());
		}

		//Og hvis skipet ikke skal til losseplass, så skal den til en kaiplass
		else if (AnchorageHarbour.Ships.Count != 0 && AnchorageHarbour.Ships.First().PlaceDestination.Id == shipPlaces.Id && shipPlaces.AvailableSpace)
		{
			//Her så fjerne vi skipet fra ankerplassen ved bruk av MoveShip metoden og
			//plasserer det til destinasjonen ved bruk AddSpesificPlace metoden
			currentDateTime.AddMinutes(30);
            AnchorageHarbour.Ships.First().AddHistory(new HistoryService(AnchorageHarbour.ShipQueue.First().ShipName, currentDateTime, shipPlaces.Name));
			RaiseDepartingAnchorage(AnchorageHarbour.Ships.First());
			AddToSpesificPlace(shipPlaces.Id, AnchorageHarbour.MoveShip(AnchorageHarbour.Ships.First().Id));
		}
	}

	private void RaiseArrivedToHarbour(Ship ship)
	{
		ArrivedToHarbour?.Invoke(this, new ArrivedToHarbourArgs(ship));
	}

	private void RaiseDepartingAnchorage(Ship ship)
	{
		DepartingAnchorage?.Invoke(this, new DepartingAnchorageArgs(ship));
	}

	private void RaiseMidnightStatusUpdate(Ship ship)
	{
		MidnightStatusUpdate?.Invoke(this, new MidnightStatusUpdateArgs(ship));
	}

	private void RaiseMovingToAnchorage(Ship ship)
	{
		MovingToAnchorage?.Invoke(this, new MovingToAnchorageArgs(ship));
	}

	private void RaiseReachedDestination(Ship ship)
	{
		ReachedDestination?.Invoke(this, new ReachedDestinationArgs(ship));
	}

	private void RaiseShipSailing(Ship ship)
	{
		ShipSailing?.Invoke(this, new ShipSailingArgs(ship));
	}
}