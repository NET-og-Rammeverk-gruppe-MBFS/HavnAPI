using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;
using System.Collections.ObjectModel;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations;
public class Harbour : IHarbour
{
	public string name { get; }
	/// <summary>
	/// Liste over alle loggførte plassering for hver skip
	/// </summary>
	public List<HistoryService> shipHistory { get; private set; }
	/// <summary>
	/// Liste over alle loggførte plassering for hver container
	/// </summary>
	public List<HistoryService> containerHistory { get; private set; }
	/// <summary>
	/// Liste over alle plasser i havnen
	/// </summary>
	public List<ShipPlaces> shipPlacesList;
	/// <summary>
	/// Liste over alle skip i havnen
	/// </summary>
	public List<Ship> shipsList { get; }
	private Anchorage anchorageHarbour;

	public event EventHandler<ArrivedToHarbourArgs> arrivedToHarbour;
	public event EventHandler<DepartingAnchorageArgs> departingAnchorage;
    public event EventHandler<MidnightStatusUpdateArgs> midnightStatusUpdate;
	public event EventHandler<MovingToAnchorageArgs> movingToAnchorage;
	public event EventHandler<ReachedDestinationArgs> reachedDestination;
	public event EventHandler<ShipSailingArgs> shipSailing;

    /// <summary>
    /// For å lage havn objekt
    /// </summary>
    /// <param name="ships"> Liste med Ship objekter som blir lagt til denne objektet.</param>
    /// <param name="shipPlaces">Liste med objekter som Unloadingspace og Dockspace. OBS når du skal lage en liste, så må du bruke ShipPlace som datatype</param>
    /// <param name="harbourName">Navnet til havnen</param>
    /// <param name="spacesInAnchorage">Antall plasser i venteplassen</param>
    /// <exception cref="InvalidNameException">Navnet på havnet kan ikke være tomt</exception>
    /// <exception cref="InvalidSpacesException">Antall plasser må være større enn 0.</exception>
    public Harbour(string harbourName, int spacesInAnchorage, List<Ship> ships, List<ShipPlaces> shipPlaces)
	{
		if (string.IsNullOrEmpty(harbourName))
		{
            throw new InvalidNameException("Name can't be null or empty");
        }

		if (spacesInAnchorage <= 0)
		{
            throw new InvalidAmountException("SpacesInAnchorage must be greater than 0");
        }

		shipsList = new List<Ship>(ships);
		shipPlacesList = new List<ShipPlaces>(shipPlaces);
		shipHistory = new List<HistoryService>();
		containerHistory = new List<HistoryService>();
		anchorageHarbour = new Anchorage(harbourName + " venteplass", spacesInAnchorage, ShipType.All);
		name = harbourName;
	}

	/// <summary>
    /// For å lage havn objekt
    /// </summary>
    /// <param name="harbourName">Navnet til havnen</param>
    /// <param name="spacesInAnchorage">Antall plasser i venteplassen</param>
    /// <exception cref="InvalidNameException">Navnet på havnet kan ikke være tomt</exception>
    /// <exception cref="InvalidSpacesException">Antall plasser må være større enn 0.</exception>
	public Harbour(string harbourName, int spacesInAnchorage)
	{
		if (string.IsNullOrEmpty(harbourName))
		{
            throw new InvalidNameException("Name can't be null or empty");
        }

		if (spacesInAnchorage <= 0)
		{
            throw new InvalidAmountException("SpacesInAnchorage must be greater than 0");
        }

		shipsList = new List<Ship>();
		shipPlacesList = new List<ShipPlaces>();
		shipHistory = new List<HistoryService>();
		containerHistory = new List<HistoryService>();
		anchorageHarbour = new Anchorage(harbourName + " venteplass", spacesInAnchorage, ShipType.All);
		name = harbourName;
	}

	/// <summary>
	/// Metode for å fjerne et skip
	/// </summary>
	/// <param name="ship">skipet som skal bli fjernet</param>
	public void RemoveShip(Ship ship)
	{
		shipsList.Remove(ship);
	}

	/// <summary>
	/// Metode for å fjerne alle skipene fra havn objektet
	/// </summary>
	public void RemoveAllShip()
	{
		shipsList.Clear();
	}

	/// <summary>
	/// Metode for å legge til Unlaodingspace eller Dockspace
	/// </summary>
	/// <param name="shipPlace">Unloadingspace eller Dockspace objekt</param>
	public void AddShipPlace(ShipPlaces shipPlace)
	{
		shipPlacesList.Add(shipPlace);
	}

	/// <summary>
	/// Metode for å legge til alle objektene som arver ShipPlace som Unloadingspace og Dockspace
	/// </summary>
	/// <param name="shipPlaces">Liste med Unloadingspace og/eller Dockspace objekter</param>
	public void AddAllShipPlaces(List<ShipPlaces> shipPlaces)
	{
		shipPlacesList.AddRange(shipPlaces);
	}

	/// <summary>
	/// Metoden legger til et skip til listen
	/// </summary>
	/// <param name="ship"></param>
	public void AddShip(Ship ship)
	{
		shipsList.Add(ship);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="allships"></param>
	public void AddAllShips(List<Ship> allships)
	{
		shipsList.AddRange(allships);
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

		foreach (Ship ship in shipsList)
		{
			if (ship.repeat)
			{
				if (ship.daily == null && ship.weekly != null)
				{
					int startDay = (int)currentTime.DayOfWeek;
    				int target = (int)ship.weekly;
    				if (target < startDay)
						target += 7;
					ship.currentRepeatedDateTime = currentTime.AddDays(target - startDay);
				}
				else if (ship.daily != null && ship.weekly == null)
				{
					if (ship.daily < TimeOnly.FromDateTime(currentTime))
					{
                        ship.currentRepeatedDateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day+1, ship.daily.Value.Hour, ship.daily.Value.Minute, ship.daily.Value.Second);
					}
					else
						ship.currentRepeatedDateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, ship.daily.Value.Hour, ship.daily.Value.Minute, ship.daily.Value.Second);
				}
			}
		}

		//Så starter simulasjonen ved bruk av while, der den vil kjøre til sluttdato-en
		while (currentTime < end && (shipsList.Count + anchorageHarbour.ships.Count + anchorageHarbour.shipQueue.Count) != 0)
		{
			//Begge for-loops under går gjennom alle ship og plassene
			foreach (ShipPlaces ShipPlace in shipPlacesList)
			{
                MoveShipFromAnchorage(ShipPlace, currentTime);
                foreach (Ship ship in new List<Ship>(shipsList))
				{
					//Her så vil de se om destienasjonen til skipet og plassen som den itererer
					if ((ship.placeDestination.id == ShipPlace.id) && (ship.spesificDateTime <= currentTime || ship.currentRepeatedDateTime <= currentTime))
					{
						if (ship.daily == null && ship.weekly != null)
							ship.currentRepeatedDateTime = ship.currentRepeatedDateTime.Value.AddDays(7);
						else if (ship.daily != null && ship.weekly == null)
							ship.currentRepeatedDateTime = ship.currentRepeatedDateTime.Value.AddDays(1);
						//Før det så lager denne metoden Containers objekters til shipet basert på antall i konstruktøren
						ship.currentLocation = name;
                    	ship.MakeContainers();
						ship.status = Status.Busy;
                        RaiseArrivedToHarbour(ship);
                        RaiseShipSailing(ship);
                        //Det skjekker om det er ledig plass i plasssen fra for loop-en
                        if (ShipPlace.AvailableSpace)
						{
							ship.AddHistory(new HistoryService(ship.shipName, currentTime.AddMinutes(60), ShipPlace.name));
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
                        
                }
				//Her så starter vi losse-prossessen
				//If testen sjekker om losseplassen er full og at det er en losseplass før man starter prossessen
				if (ShipPlace is Unloadingspace)
				{
					//Her så legger vi til miutter basert på hvor mange Container objekter det er i en skip, hvor mange
					//skip det er i losseplassen, og hvor fort losse-prossessen er basert på bruker av API-et
					((Unloadingspace)ShipPlace).UnloadContainer(currentTime, end);
					((Unloadingspace)ShipPlace).targetContainerSpace.OverdueContainers(currentTime, end);
					//Etter at alle skipene i losseplassen er ferdig, så returnerer vi listen tilbake til havn klassen
					AddAllShips(((Unloadingspace)ShipPlace).ReturnRepeatingShips());

					//Vi antar at når skipene har blir returnert til havn klassen, så seiler de til Start-of-sea passage som tar 60 min

				}
				if (ShipPlace is Dockspace)
				{
					AddAllShips(((Dockspace)ShipPlace).ReturnRepeatingShips());
                }
			}
			if (currentTime.Hour == 0)
			{
				RaiseMidnightStatusUpdate(getAllReadOnlyShips());
			}
            //Etter en flere iterasjoner, så antar vi at alle skipene har seilet samtidig. Da legger vi 60 minutter for hver gang flere skip
            //har nådd destinasjonen
            currentTime = currentTime.AddMinutes(60);
        }

		foreach (ShipPlaces shipPlaces in shipPlacesList)
		{
			shipsList.AddRange(shipPlaces.ReturnAllShips());
			if(shipPlaces is Unloadingspace)
			{
				containerHistory.AddRange(((Unloadingspace)shipPlaces).containerHistory);
			}
		}
		foreach (Ship ship1 in shipsList)
		{
			shipHistory.AddRange(ship1.histories);
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
		shipsList.Remove(theShip);
		return Ship;
	}


	/// <summary>
	/// Det blir brukt i simulasjonen for å legge et skip til et spesifikk plass
	/// </summary>
	/// <param name="ShipPlaceId"></param>
	/// <param name="ship"></param>
	private void AddToSpesificPlace(int shipPlaceId, Ship ship)
	{
		foreach (ShipPlaces Shipplace in shipPlacesList)
		{
			if(shipPlaceId == Shipplace.id)
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
		if (ship.placeDestination is Unloadingspace)
		{
			CurrentDateTime = CurrentDateTime.AddMinutes(30);
			ship.AddHistory(new HistoryService(ship.shipName, CurrentDateTime, anchorageHarbour.name));
			RaiseMovingToAnchorage(ship);
			ship.status = Status.Available;
			ship.currentLocation = anchorageHarbour.name;
            anchorageHarbour.AddShipToQueue(MoveShip(ship));
		}
		else if (ship.placeDestination is Dockspace)
		{
			CurrentDateTime = CurrentDateTime.AddMinutes(30);
			ship.AddHistory(new HistoryService(ship.shipName, CurrentDateTime, anchorageHarbour.name));
            RaiseMovingToAnchorage(ship);
			ship.status = Status.Available;
			ship.currentLocation = anchorageHarbour.name;
            anchorageHarbour.AddShip(MoveShip(ship));
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
		if (anchorageHarbour.shipQueue.Count != 0 && anchorageHarbour.shipQueue.Peek().placeDestination.id == shipPlaces.id && shipPlaces.AvailableSpace)
		{
			//Her så fjerne vi skipet fra ankerplassen ved bruk av MoveShipFromQueue metoden og
			//plasserer det til destinasjonen ved bruk AddSpesificPlace metoden
			currentDateTime.AddMinutes(30);
            anchorageHarbour.shipQueue.Peek().AddHistory(new HistoryService(anchorageHarbour.shipQueue.Peek().shipName, currentDateTime, shipPlaces.name));
			RaiseDepartingAnchorage(anchorageHarbour.shipQueue.Peek());
			anchorageHarbour.shipQueue.Peek().status = Status.Busy;
			AddToSpesificPlace(shipPlaces.id, anchorageHarbour.MoveShipFromQueue());
		}

		//Og hvis skipet ikke skal til losseplass, så skal den til en kaiplass
		else if (anchorageHarbour.ships.Count != 0 && anchorageHarbour.ships.First().placeDestination.id == shipPlaces.id && shipPlaces.AvailableSpace)
		{
			//Her så fjerne vi skipet fra ankerplassen ved bruk av MoveShip metoden og
			//plasserer det til destinasjonen ved bruk AddSpesificPlace metoden
			currentDateTime.AddMinutes(30);
            anchorageHarbour.ships.First().AddHistory(new HistoryService(anchorageHarbour.shipQueue.First().shipName, currentDateTime, shipPlaces.name));
			RaiseDepartingAnchorage(anchorageHarbour.ships.First());
			anchorageHarbour.shipQueue.Peek().status = Status.Busy;
			AddToSpesificPlace(shipPlaces.id, anchorageHarbour.MoveShip(anchorageHarbour.ships.First().id));
		}
	}

	/// <summary>
	/// En metode som henter alle skipene fra
	/// </summary>
	/// <returns></returns>
	private IReadOnlyCollection<Ship> getAllReadOnlyShips()
	{
		List<Ship> temp = new List<Ship>();
		temp.AddRange(anchorageHarbour.ships);
		temp.AddRange(shipsList);
		temp.AddRange(anchorageHarbour.shipQueue);
		foreach (ShipPlaces shipPlaces in shipPlacesList)
		{
			temp.AddRange(shipPlaces.ships);
			temp.AddRange(shipPlaces.finished);
		}
		return new ReadOnlyCollection<Ship>(temp);
	}

	private void RaiseArrivedToHarbour(Ship ship)
	{
		arrivedToHarbour?.Invoke(this, new ArrivedToHarbourArgs(ship));
	}

	private void RaiseDepartingAnchorage(Ship ship)
	{
		departingAnchorage?.Invoke(this, new DepartingAnchorageArgs(ship));
	}

	private void RaiseMidnightStatusUpdate(IReadOnlyCollection<Ship> ship)
	{
		midnightStatusUpdate?.Invoke(this, new MidnightStatusUpdateArgs(ship));
	}

	private void RaiseMovingToAnchorage(Ship ship)
	{
		movingToAnchorage?.Invoke(this, new MovingToAnchorageArgs(ship));
	}

	private void RaiseReachedDestination(Ship ship)
	{
		reachedDestination?.Invoke(this, new ReachedDestinationArgs(ship));
	}

	private void RaiseShipSailing(Ship ship)
	{
		shipSailing?.Invoke(this, new ShipSailingArgs(ship));
	}
}