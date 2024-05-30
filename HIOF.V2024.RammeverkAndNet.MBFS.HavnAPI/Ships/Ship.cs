namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;
using System;
using System.Collections.ObjectModel;

public class Ship
{
    /// <summary>
    /// ID til et skip. ID er autogenerert
    /// </summary>
    public int Id { get; private set;}
    /// <summary>
    /// Navnet til skipet
    /// </summary>
    public string ShipName { get; private set; }
    /// <summary>
    /// Destinasjonen til skipet. Dette er et ShipPlace objekt
    /// </summary>
    public ShipPlaces PlaceDestination { get; private set; }
    /// <summary>
    /// Tiden når skipet skal ankomme destinasjonen i et spesifikk tid (Ikke gjentagende seiling)
    /// </summary>
    public Nullable<DateTime> SpesificDateTime { get; private set; } = null;
    /// <summary>
    /// Gjentagende seiling for hver uke.
    /// </summary>
    public Nullable<DayOfWeek> Weekly { get; private set; } = null;
    /// <summary>
    /// Gjentagende seiling for hver dag
    /// </summary>
    public Nullable<TimeOnly> Daily { get; private set; } = null;
    /// <summary>
    /// Nåværende status til skipet
    /// </summary>
    public Status Status {get; internal set; } = Status.Available;
    /// <summary>
    /// Totalt antall containere i skipet
    /// </summary>
    public int TotalContainers { get; private set; }
    /// <summary>
    /// Det forteller hva slags type skip det er
    /// </summary>
    public ShipType Type { get; private set; }

    /// <summary>
    /// Liste over alle loggførte plassering til skip.
    /// </summary>
    public ReadOnlyCollection<HistoryService> Histories
  	{
    	get { return new ReadOnlyCollection<HistoryService>(HistoriesInternal); }
  	}

    /// <summary>
    /// Nåværende sted til skipet. Returnerer kun navnet til plassen
    /// </summary>
    public string CurrentLocation { get; internal set; }


    /// <summary>
    /// For laget et skip
    /// </summary>
    /// <param name="shipname">Navnet til skipet</param>
    /// <param name="placedestination"> Destinasjon til skipet. OBS du må ha en dockspace eller unloadingspace objekt før du kan lage skip objektet</param>
    /// <param name="spesificDateTimeSailing">Når skipet skal nå destinasjon i et spesifikk tid</param>
    /// <param name="amountOfLongContainers">Antall lange ISO containere</param>
    /// <param name="amountOfShortContainers">Antall korte ISO containere</param>
    /// <param name="shipType">Type for hva skal type skip det er</param>
    /// <exception cref="InvalidNameException">Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidDestinationException">hvis du referer til et destinasjon som ikke eksisterer</exception>
    /// <exception cref="InvalidAmountOfContainersException">Hvis du gir ugyldig antall korte og/eller lange ISO containere som f.eks -1</exception>
    /// <exception cref="InvalidShipTypeDestinationException">Hvis du gir skip objektet et destinasjon som ikke tillater ship-en med typen du valgte for dette skip objektet</exception>
    public Ship(SimulationName shipname, ShipPlaces placedestination, DateTime spesificDateTimeSailing, int amountOfLongContainers, int amountOfShortContainers, ShipType shipType)
    {
        if (string.IsNullOrEmpty(shipname.ToString()))
        {
            throw new InvalidNameException("ShipName cannot be empty.");
        }

        if (placedestination == null)
        {
            throw new InvalidDestinationException("ShipDestination cannot be null.");
        }

        if (amountOfLongContainers < 0 || amountOfShortContainers < 0)
        {
            throw new InvalidAmountOfContainersException("AmountOfContainers must be greater than or equal to 0.");
        }

        if (placedestination.ShipType != ShipType.All && shipType != placedestination.ShipType)
        {
            throw new InvalidShipTypeDestinationException("ShipType must be the same as the destination type, or destination must allow all types.");
        }

        Id = Interlocked.Increment(ref Next);
        ShipName = shipname.ToString();
        PlaceDestination = placedestination;
        SpesificDateTime = spesificDateTimeSailing;
        Repeat = false;
        Containers = new Queue<Container>();
        HistoriesInternal = new Collection<HistoryService>();
        AmountLongContainers = amountOfLongContainers;
        AmountShortContainers = amountOfShortContainers;
        TotalContainers = amountOfLongContainers + amountOfShortContainers;
        Type = shipType;
    }

    /// <summary>
    /// For laget et skip
    /// </summary>
    /// <param name="shipname">Navnet til skipet</param>
    /// <param name="placedestination"> Destinasjon til skipet. OBS du må ha en dockspace eller unloadingspace objekt før du kan lage skip objektet</param>
    /// <param name="weeklySailing">Ukentlig seilinger basert på bestemt dag i uken f.eks Onsdag</param>
    /// <param name="amountOfLongContainers">Antall lange ISO containere</param>
    /// <param name="amountOfShortContainers">Antall korte ISO containere</param>
    /// <param name="shipType">Type for hva skal type skip det er</param>
    /// <exception cref="InvalidNameException">Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidDestinationException">hvis du referer til et destinasjon som ikke eksisterer</exception>
    /// <exception cref="InvalidAmountOfContainersException">Hvis du gir ugyldig antall korte og/eller lange ISO containere som f.eks -1</exception>
    /// <exception cref="InvalidShipTypeDestinationException">Hvis du gir skip objektet et destinasjon som ikke tillater ship-en med typen du valgte for dette skip objektet</exception>
    public Ship(SimulationName shipname, ShipPlaces placedestination, DayOfWeek weeklySailing, int amountOfLongContainers, int amountOfShortContainers, ShipType shipType)
    {
        if (string.IsNullOrEmpty(shipname.ToString()))
        {
            throw new InvalidNameException("ShipName cannot be empty.");
        }

        if (placedestination == null)
        {
            throw new InvalidDestinationException("ShipDestination cannot be null.");
        }

        if (amountOfLongContainers < 0 || amountOfShortContainers < 0)
        {
            throw new InvalidAmountOfContainersException("AmountOfContainers must be greater than or equal to 0.");
        }

        if (placedestination.ShipType != ShipType.All && shipType != placedestination.ShipType)
        {
            throw new InvalidShipTypeDestinationException("ShipType must be the same as the destination type, or destination must allow all types.");
        }

        Id = Interlocked.Increment(ref Next);
        ShipName = shipname.ToString();
        PlaceDestination = placedestination;
        Weekly = weeklySailing;
        Repeat = true;
        Containers = new Queue<Container>();
        HistoriesInternal = new Collection<HistoryService>();
        AmountLongContainers = amountOfLongContainers;
        AmountShortContainers = amountOfShortContainers;
        TotalContainers = amountOfLongContainers + amountOfShortContainers;
        Type = shipType;
    }

    /// <summary>
    /// For laget et skip
    /// </summary>
    /// <param name="shipname">Navnet til skipet</param>
    /// <param name="placedestination"> Destinasjon til skipet. OBS du må ha en dockspace eller unloadingspace objekt før du kan lage skip objektet</param>
    /// <param name="dailySailing">Daglig seilinger basert på et bestemt tid på dagen f.eks 14:00</param>
    /// <param name="amountOfLongContainers">Antall lange ISO containere</param>
    /// <param name="amountOfShortContainers">Antall korte ISO containere</param>
    /// <param name="shipType">Type for hva skal type skip det er</param>
    /// <exception cref="InvalidNameException">Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidDestinationException">hvis du referer til et destinasjon som ikke eksisterer</exception>
    /// <exception cref="InvalidAmountOfContainersException">Hvis du gir ugyldig antall korte og/eller lange ISO containere som f.eks -1</exception>
    /// <exception cref="InvalidShipTypeDestinationException">Hvis du gir skip objektet et destinasjon som ikke tillater ship-en med typen du valgte for dette skip objektet</exception>
    public Ship(SimulationName shipname, ShipPlaces placedestination, TimeOnly dailySailing, int amountOfLongContainers, int amountOfShortContainers, ShipType shipType)
    {
        if (string.IsNullOrEmpty(shipname.ToString()))
        {
            throw new InvalidNameException("ShipName cannot be empty.");
        }

        if (placedestination == null)
        {
            throw new InvalidDestinationException("ShipDestination cannot be null.");
        }

        if (amountOfLongContainers < 0 || amountOfShortContainers < 0)
        {
            throw new InvalidAmountOfContainersException("AmountOfContainers must be greater than or equal to 0.");
        }

        if (placedestination.ShipType != ShipType.All && shipType != placedestination.ShipType)
        {
            throw new InvalidShipTypeDestinationException("ShipType must be the same as the destination type, or destination must allow all types.");
        }

        Id = Interlocked.Increment(ref Next);
        ShipName = shipname.ToString();
        PlaceDestination = placedestination;
        Daily = dailySailing;
        Repeat = true;
        Containers = new Queue<Container>();
        HistoriesInternal = new Collection<HistoryService>();
        AmountLongContainers = amountOfLongContainers;
        AmountShortContainers = amountOfShortContainers;
        TotalContainers = amountOfLongContainers + amountOfShortContainers;
        Type = shipType;
    }

    internal int AmountLongContainers { get; set; }
    internal int AmountShortContainers { get; set; }
    internal Collection<HistoryService> HistoriesInternal { get; set; }
    internal Queue<Container> Containers { get; private set; }
    internal Nullable<DateTime> CurrentRepeatedDateTime { get; set; } = null;
    internal bool Repeat { get; set; }
    private static int Next = 0;

    internal void MakeContainers()
    {
        Containers.Clear();
        for (int i = 0; i < AmountLongContainers; i++)
        {
            Containers.Enqueue(new Container(ContainerType.Long));
        }
        for (int i = 0; i < AmountShortContainers; i++)
        {
            Containers.Enqueue(new Container(ContainerType.Short));
        }
    }

    /// <summary>
    /// Fjerner container objekt fra kø
    /// </summary>
    /// <returns container etter å ha blitt fjernet></returns>
    internal Container MoveContainer()
    {
        return Containers.Dequeue();
    }
    
    /// <summary>
    /// legger til en historie til skipet
    /// </summary>
    /// <param name="history">Historikk objektet som skal bli lagt til</param>
    internal void AddHistory(HistoryService history)
    {
        HistoriesInternal.Add(history);
        
    }

    /// <summary>
    /// Fjerner en historie fra skipet
    /// </summary>
    /// <param name="history">Historikk objektet som skal bli slettet</param>
    internal void RemoveHistory(HistoryService history)
    {
        HistoriesInternal.Remove(history);
    }
}