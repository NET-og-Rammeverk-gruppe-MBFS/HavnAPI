namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Unloadingspace : ShipPlaces
{
    private int Cranes { get; set; }
    private Queue<Container> TempContainers { get; }
    internal List<Container> containerSaved { get; }
    public double TruckPickupPercentage { get; set; }

    public Unloadingspace(string Name, int Spaces, int cranes, double truckPickupPercentage) : base(Name, Spaces)
    {
        if (cranes < Spaces)
        {
            throw new InvalidAmountOfCranesPerSpacesException("The amount of cranes can't be less than the amount of spaces");
        }

        containerSaved = new List<Container>();
        TempContainers = new Queue<Container>();
        Cranes = cranes;
        TruckPickupPercentage = truckPickupPercentage;
    }

    /// <summary>
    /// Metoden legger til en container i losseplassen fra shipene som er i Ship listen, og fjerne containers fra plassene basert på emptyFrequency
    /// <summary>
    /// <param name="currentDateTime"> Det blir brukt for å lagre tiden i historikken til en container objekt under simulasjonen</param>
    internal int UnloadContainer(DateTime currentDateTime, DateTime end)
    {
        DateTime start = currentDateTime;
        var totalUnloadTime = 0;
        int TrucksDispatched = 0;
        foreach (var ship in new List<Ship>(Ships))
        {
            int ContainersToUnload = ship.containers.Count;
            int truckContainers = (int)(ContainersToUnload * TruckPickupPercentage / 100);
            int agvContainers = ContainersToUnload - truckContainers;

            while (ship.containers.Count > 0 && start < end)
            {
                int TimePerContainer = Math.Max(1, 5 / Cranes);
                totalUnloadTime += TimePerContainer;
                start = start.AddMinutes(TimePerContainer);

                Container container = ship.MoveContainer();
                container.Histories.Add(new HistoryService(Name, start));

                if (truckContainers > 0)
                {
                    TrucksDispatched++;
                    truckContainers--;
                }
                else if (agvContainers > 0)
                {
                    //mangler logikk for AGV og containerplass
                }
                ContainersToUnload--;
            }
            if (ship.Repeat == false)
            {
                Finished.Add(ship);
                Ships.Remove(ship);
            }

        }
        return totalUnloadTime;
    }

    internal override void AddShip(Ship ship)
    {
        Ships.Add(ship);
    }
}