namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;

using System;
using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Unloadingspace : ShipPlaces
{
    private int Cranes { get; set; }
    public double TruckPickupPercentage { get; set; }
    internal Collection<HistoryService> ContainerHistory = new Collection<HistoryService>();
    internal ContainerSpace TargetContainerSpace { get; set; }

    public Unloadingspace(string Name, int Spaces, ShipType Type, int cranes, double truckPickupPercentage, ContainerSpace targetContainerSpace) : base(Name, Spaces, Type)
    {
        if (cranes < Spaces)
        {
            throw new InvalidAmountOfCranesPerSpacesException("The amount of cranes can't be less than the amount of spaces");
        }

        Cranes = cranes;
        TruckPickupPercentage = truckPickupPercentage;
        TargetContainerSpace = targetContainerSpace;
    }

    /// <summary>
    /// Metoden laster av containere fra skipene som er i havnen til lastebiler og AGVer
    /// <summary>
    /// <param name="currentDateTime"> Det blir brukt for å lagre tiden i historikken til en container objekt under simulasjonen</param>
    /// <param name="end"> Tiden som simulasjonen skal stoppe</param>
    internal int UnloadContainer(DateTime currentDateTime, DateTime end)
    {
        Random random = new Random();
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
                container.Histories.Add(new HistoryService("Container " + container.ID, start, Name));
                ContainerHistory.Add(new HistoryService("Container " + container.ID, start, Name));

                if (truckContainers > 0)
                {
                    TrucksDispatched++;
                    truckContainers--;
                }
                else if (agvContainers > 0)
                {
                    AGV agv = TargetContainerSpace.AGVs.FirstOrDefault(a => a.status == Status.Available);
                    if (agv != null)
                    {
                        agv.container = container;
                        agv.status = Status.Busy;

                        start = start.AddMinutes(1);

                        StorageColumn storageColumn = TargetContainerSpace.StorageColumns[random.Next(TargetContainerSpace.StorageColumns.Count)];
                        Column column = storageColumn.Columns[random.Next(storageColumn.Columns.Count)];

                        column.AddContainer(agv.container);
                        container.Histories.Add(new HistoryService("Container " + container.ID, start, Name + " StorageColumn"));
                        ContainerHistory.Add(new HistoryService("Container " + container.ID, start, Name + " StorageColumn"));
                        agv.container = null;
                        agv.status = Status.Available;

                    }
                    agvContainers--;
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