namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;
using System;
using System.Collections.ObjectModel;

public class Ship
{
    private static int Next = 0;
    public int Id { get; }
    public string ShipName { get; private set; }
    public ShipPlaces PlaceDestination { get; }
    public DateTime ArrivalTime { get; set; }
    internal bool Repeat { get; set; }
    public int AmountContainers { get; private set; }

    internal Queue<Container> containers { get; private set; }
    internal Collection<HistoryService> histories { get; private set; }


    /// <summary>
    /// initlisere en ny instans av <see cref="Ship"/>- classe for � holde styr p� skipets informasjon.
    /// </summary>
    /// <param name="id"> id for ship</param>
    /// <param name="shipname"> navnet på shipet</param>
    /// <param name="placedestination">destiniasjonen til shipet</param>
    /// <param name="arrivalTime">ankomst tid for shipet</param>
    /// <param name="repeat"> verdi som vi setter inn om turen skal gjenta seg</param>
    /// <exception cref="InvalidNameException">Kastes hvis ShipName er tom.</exception>
    /// <exception cref="InvalidPlaceDestinationException">Kastes hvis PlaceDestination er tom.</exception>
    /// <exception cref="InvalidAmountOfContainersException">Kastes hvis AmountContainers er mindre enn 0.</exception>"
    public Ship(string shipname, ShipPlaces placedestination, DateTime arrivalTime, bool repeat, int ammountOfContainers)
    {
        if (string.IsNullOrEmpty(shipname))
        {
            throw new InvalidNameException("ShipName cannot be empty");
        }

        if (placedestination == null)
        {
            throw new InvalidDestinationException("ShipDestination cannot be null");
        }

        if (ammountOfContainers < 0)
        {
            throw new InvalidAmountOfContainersException("AmountContainers must be greater than or equal to 0");
        }

        Id = Interlocked.Increment(ref Next);
        ShipName = shipname;
        PlaceDestination = placedestination;
        ArrivalTime = arrivalTime;
        Repeat = repeat;
        containers = new Queue<Container>();
        histories = new Collection<HistoryService>();
        AmountContainers = ammountOfContainers;

        
    }

    /// <summary>
    /// Legger til en container til skipet
    /// </summary>
    /// <param name="container"></param>
    internal void MakeContainers ()
    {
        containers.Clear();
        for (int i = 0; i < AmountContainers; i++)
        {
            containers.Enqueue(new Container());
        }

    }

    /// <summary>
    /// Fjerner container objekt fra kø
    /// </summary>
    /// <returns container etter å ha blitt fjernet></returns>
    internal Container MoveContainer()
    {
        return containers.Dequeue();
    }
    
    /// <summary>
    /// legger til en historie til skipet
    /// </summary>
    /// <param name="history"></param>
    internal void AddHistory(HistoryService history)
    {
        histories.Add(history);
        
    }

    /// <summary>
    /// Fjerner en historie fra skipet
    /// </summary>
    /// <param name="history"></param>
    internal void RemoveHistory(HistoryService history)
    {
        histories.Remove(history);
    }
}