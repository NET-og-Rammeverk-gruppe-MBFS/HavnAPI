using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations;
public class Harbour : IHarbour
{
    /// <summary>
    /// Navnet til havnen
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Liste over alle loggførte plassering for hver skip
    /// </summary>
    public ReadOnlyCollection<HistoryService> ShipHistory
    {
        get { return new ReadOnlyCollection<HistoryService>(ShipHistoryInternal); }
    }

    /// <summary>
    /// Liste over alle loggførte plassering for hver container
    /// </summary>
    public ReadOnlyCollection<HistoryService> ContainerHistory
    {
        get { return new ReadOnlyCollection<HistoryService>(ContainerHistoryInternal); }
    }

    /// <summary>
    /// Liste over alle plasser i havnen
    /// </summary>
    public Collection<ShipPlaces> ShipPlacesList { get; private set; }
    /// <summary>
    /// Liste over alle skip i havnen
    /// </summary>
    public Collection<Ship> ShipsList { get; private set; }

    /// <summary>
    /// For å lage havn objekt
    /// </summary>
    /// <param name="ships"> Liste med Ship objekter som blir lagt til denne objektet.</param>
    /// <param name="shipPlaces">Liste med objekter som Unloadingspace og Dockspace. OBS når du skal lage en liste, så må du bruke ShipPlace som datatype</param>
    /// <param name="harbourName">Navnet til havnen</param>
    /// <param name="spacesInAnchorage">Antall plasser i venteplassen</param>
    /// <exception cref="InvalidNameException">Navnet på havnet kan ikke være tomt</exception>
    /// <exception cref="InvalidSpacesException">Antall plasser må være større enn 0.</exception>
    public Harbour(SimulationName harbourName, int spacesInAnchorage, List<Ship> ships, List<ShipPlaces> shipPlaces)
    {
        if (string.IsNullOrEmpty(harbourName.ToString()))
        {
            throw new InvalidNameException("Name can't be null or empty.");
        }

        if (spacesInAnchorage <= 0)
        {
            throw new InvalidAmountException("SpacesInAnchorage must be greater than 0.");
        }

        ShipsList = new Collection<Ship>(ships);
        ShipPlacesList = new Collection<ShipPlaces>(shipPlaces);
        ShipHistoryInternal = new Collection<HistoryService>();
        ContainerHistoryInternal = new Collection<HistoryService>();
        AnchorageHarbour = new Anchorage(new SimulationName(harbourName.ToString() + " venteplass"), spacesInAnchorage, ShipType.All);
        Name = harbourName.ToString();
    }

    /// <summary>
    /// For å lage havn objekt
    /// </summary>
    /// <param name="harbourName">Navnet til havnen</param>
    /// <param name="spacesInAnchorage">Antall plasser i venteplassen</param>
    /// <exception cref="InvalidNameException">Navnet på havnet kan ikke være tomt</exception>
    /// <exception cref="InvalidSpacesException">Antall plasser må være større enn 0.</exception>
    public Harbour(SimulationName harbourName, int spacesInAnchorage)
    {
        if (string.IsNullOrEmpty(harbourName.ToString()))
        {
            throw new InvalidNameException("Name can't be null or empty.");
        }

        if (spacesInAnchorage <= 0)
        {
            throw new InvalidAmountException("SpacesInAnchorage must be greater than 0.");
        }

        ShipsList = new Collection<Ship>();
        ShipPlacesList = new Collection<ShipPlaces>();
        ShipHistoryInternal = new Collection<HistoryService>();
        ContainerHistoryInternal = new Collection<HistoryService>();
        AnchorageHarbour = new Anchorage(new SimulationName(harbourName.ToString() + " venteplass"), spacesInAnchorage, ShipType.All);
        Name = harbourName.ToString();
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
            throw new InvalidDateTimeRangeException("End date must be greater than start date.", nameof(end));
        }

        DateTime currentTime = start;

        foreach (Ship ship in ShipsList)
        {
            if (ship.Repeat)
            {
                if (ship.Daily == null && ship.Weekly != null)
                {
                    int startDay = (int)currentTime.DayOfWeek;
                    int target = (int)ship.Weekly;
                    if (target < startDay)
                        target += 7;
                    ship.CurrentRepeatedDateTime = currentTime.AddDays(target - startDay);
                }

                else if (ship.Daily != null && ship.Weekly == null)
                {
                    if (ship.Daily < TimeOnly.FromDateTime(currentTime))
                    {
                        ship.CurrentRepeatedDateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day+1, ship.Daily.Value.Hour, ship.Daily.Value.Minute, ship.Daily.Value.Second);
                    }

                    else
                        ship.CurrentRepeatedDateTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, ship.Daily.Value.Hour, ship.Daily.Value.Minute, ship.Daily.Value.Second);
                }
            }
        }

        while (currentTime < end && (ShipsList.Count + AnchorageHarbour.Ships.Count + AnchorageHarbour.ShipQueue.Count) != 0)
        {
            foreach (ShipPlaces ShipPlace in ShipPlacesList)
            {
                MoveShipFromAnchorage(ShipPlace, currentTime);
                foreach (Ship ship in new List<Ship>(ShipsList))
                {
                    if ((ship.PlaceDestination.Id == ShipPlace.Id) && (ship.SpesificDateTime <= currentTime || ship.CurrentRepeatedDateTime <= currentTime))
                    {
                        if (ship.Daily == null && ship.Weekly != null)
                            ship.CurrentRepeatedDateTime = ship.CurrentRepeatedDateTime.Value.AddDays(7);
                        else if (ship.Daily != null && ship.Weekly == null)
                            ship.CurrentRepeatedDateTime = ship.CurrentRepeatedDateTime.Value.AddDays(1);

                        ship.CurrentLocation = Name;
                        ship.MakeContainers();
                        ship.Status = Status.Busy;
                        RaiseArrivedToHarbour(ship);
                        RaiseShipSailing(ship);

                        if (ShipPlace.AvailableSpace)
                        {
                            ship.AddHistory(new HistoryService(ship.ShipName, currentTime.AddMinutes(60), ShipPlace.Name));
                            RaiseReachedDestination(ship);
                            ShipPlace.AddShip(MoveShip(ship));
                        }

                        else
                        {
                            AddShipToAnchorage(ship, currentTime);
                        }
                    }      
                }
                
                if (ShipPlace is Unloadingspace)
                {
                    ((Unloadingspace)ShipPlace).UnloadContainer(currentTime, end);
                    ((Unloadingspace)ShipPlace).TargetContainerSpace.OverdueContainers(currentTime, end);
                    AddAllShips(((Unloadingspace)ShipPlace).ReturnRepeatingShips());
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
            currentTime = currentTime.AddMinutes(60);
        }

        foreach (ShipPlaces shipPlaces in ShipPlacesList)
        {
            AddAllShips(shipPlaces.ReturnAllShips());
            if(shipPlaces is Unloadingspace)
            {
                foreach (HistoryService containerHistory in ((Unloadingspace)shipPlaces).ContainerHistory)
                {
                    ContainerHistoryInternal.Add(containerHistory);
                }
            }
        }

        foreach (Ship ship1 in ShipsList)
        {
            foreach (HistoryService shipHistory in ship1.HistoriesInternal)
            {
                ShipHistoryInternal.Add(shipHistory);
            }
        }
    }

    public event EventHandler<ArrivedToHarbourArgs> ArrivedToHarbour;
    public event EventHandler<DepartingAnchorageArgs> DepartingAnchorage;
    public event EventHandler<MidnightStatusUpdateArgs> MidnightStatusUpdate;
    public event EventHandler<MovingToAnchorageArgs> MovingToAnchorage;
    public event EventHandler<ReachedDestinationArgs> ReachedDestination;
    public event EventHandler<ShipSailingArgs> ShipSailing;

    /// <summary>
    /// Metode for å fjerne et plass
    /// </summary>
    /// <param name="shipPlaces"></param>
    public void RemoveShipPlace(ShipPlaces shipPlaces)
    {
        ShipPlacesList.Remove(shipPlaces);
    }

    /// <summary>
    /// Metode for å fjerner alle plassene fra objektet
    /// </summary>
    public void RemoveAllShipPlaces()
    {
        ShipPlacesList.Clear();
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
    public void AddAllShipPlaces(List<ShipPlaces> allShipPlaces)
    {
        foreach (ShipPlaces shipplace in allShipPlaces)
        {
            ShipPlacesList.Add(shipplace);
        }
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
    /// Legger til en liste med skip
    /// </summary>
    /// <param name="allships"></param>
    public void AddAllShips(List<Ship> allships)
    {
        foreach (Ship ship in allships)
        {
            ShipsList.Add(ship);
        }
    }

    internal Collection<HistoryService> ShipHistoryInternal { get; set; }
    internal Collection<HistoryService> ContainerHistoryInternal { get; set; }
    private Anchorage AnchorageHarbour;

    private Ship MoveShip(Ship theShip)
    {
        Ship Ship = theShip;
        ShipsList.Remove(theShip);
        return Ship;
    }


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

    

    private void AddShipToAnchorage(Ship ship, DateTime current)
    {
        DateTime CurrentDateTime = current;
        if (ship.PlaceDestination is Unloadingspace)
        {
            CurrentDateTime = CurrentDateTime.AddMinutes(30);
            ship.AddHistory(new HistoryService(ship.ShipName, CurrentDateTime, AnchorageHarbour.Name));
            RaiseMovingToAnchorage(ship);
            ship.Status = Status.Available;
            ship.CurrentLocation = AnchorageHarbour.Name;
            AnchorageHarbour.AddShipToQueue(MoveShip(ship));
        }

        else if (ship.PlaceDestination is Dockspace)
        {
            CurrentDateTime = CurrentDateTime.AddMinutes(30);
            ship.AddHistory(new HistoryService(ship.ShipName, CurrentDateTime, AnchorageHarbour.Name));
            RaiseMovingToAnchorage(ship);
            ship.Status = Status.Available;
            ship.CurrentLocation = AnchorageHarbour.Name;
            AnchorageHarbour.AddShip(MoveShip(ship));
        }
    }


    private void MoveShipFromAnchorage(ShipPlaces shipPlaces, DateTime current)
    {
        DateTime currentDateTime = current;
        if (AnchorageHarbour.ShipQueue.Count != 0 && AnchorageHarbour.ShipQueue.Peek().PlaceDestination.Id == shipPlaces.Id && shipPlaces.AvailableSpace)
        {
            currentDateTime.AddMinutes(30);
            AnchorageHarbour.ShipQueue.Peek().AddHistory(new HistoryService(AnchorageHarbour.ShipQueue.Peek().ShipName, currentDateTime, shipPlaces.Name));
            RaiseDepartingAnchorage(AnchorageHarbour.ShipQueue.Peek());
            AnchorageHarbour.ShipQueue.Peek().Status = Status.Busy;
            AddToSpesificPlace(shipPlaces.Id, AnchorageHarbour.MoveShipFromQueue());
        }

        else if (AnchorageHarbour.Ships.Count != 0 && AnchorageHarbour.Ships.First().PlaceDestination.Id == shipPlaces.Id && shipPlaces.AvailableSpace)
        {
            currentDateTime.AddMinutes(30);
            AnchorageHarbour.Ships.First().AddHistory(new HistoryService(AnchorageHarbour.ShipQueue.First().ShipName, currentDateTime, shipPlaces.Name));
            RaiseDepartingAnchorage(AnchorageHarbour.Ships.First());
            AnchorageHarbour.ShipQueue.Peek().Status = Status.Busy;
            AddToSpesificPlace(shipPlaces.Id, AnchorageHarbour.MoveShip(AnchorageHarbour.Ships.First().Id));
        }
    }


    private IReadOnlyCollection<Ship> getAllReadOnlyShips()
    {
        List<Ship> temp = new List<Ship>();
        temp.AddRange(AnchorageHarbour.Ships);
        temp.AddRange(ShipsList);
        temp.AddRange(AnchorageHarbour.ShipQueue);
        foreach (ShipPlaces shipPlaces in ShipPlacesList)
        {
            temp.AddRange(shipPlaces.Ships);
            temp.AddRange(shipPlaces.Finished);
        }
        return new ReadOnlyCollection<Ship>(temp);
    }

    private void RaiseArrivedToHarbour(Ship ship)
    {
        ArrivedToHarbour?.Invoke(this, new ArrivedToHarbourArgs(ship));
    }

    private void RaiseDepartingAnchorage(Ship ship)
    {
        DepartingAnchorage?.Invoke(this, new DepartingAnchorageArgs(ship));
    }

    private void RaiseMidnightStatusUpdate(IReadOnlyCollection<Ship> shipList)
    {
        MidnightStatusUpdate?.Invoke(this, new MidnightStatusUpdateArgs(shipList));
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