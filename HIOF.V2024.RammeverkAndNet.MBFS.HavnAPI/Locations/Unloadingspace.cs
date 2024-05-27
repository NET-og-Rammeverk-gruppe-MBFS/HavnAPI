namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;

using System;
using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Unloadingspace : ShipPlaces
{
    private int Cranes { get; set; }
    internal double TruckPickupPercentage { get; set; }
    internal Collection<HistoryService> ContainerHistory = new Collection<HistoryService>();
    internal ContainerSpace TargetContainerSpace { get; set; }

    /// <summary>
    /// For å lage losseplass
    /// </summary>
    /// <param name="name">Navnet til losseplassen</param>
    /// <param name="shipSpaces">Antall plasser i losseplassen</param>
    /// <param name="shipType">Type skip som er tillatt i losseplassen</param>
    /// <param name="cranes">Antall kraner i losseplassen</param>
    /// <param name="truckPickupPercentage">En prosentandel av lasterbiler som frakter containere direkte ut (Resten blir fraktet til containerspace)</param>
    /// <param name="targetContainerSpace">Containerspace der containere blir lagret. OBS: Du må lage containerspace samt bruke AddStorageColumn metoden før du legger det her. Se dokumentasjonen for mer info</param>
    /// <exception cref="InvalidAmountOfCranesPerSpacesException"> Error </exception>
    /// <exception cref="InvalidNameException"> Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
    /// <exception cref="InvalidAmountException">Error for hvis du legger til ugyldig antall plasser som f.eks -1</exception>
    /// <exception cref="InvalidDestinationException">hvis du referer til et containerspace som ikke eksisterer</exception>
    public Unloadingspace(string placeName, int shipSpaces, ShipType shipType, int unloadingCranes, double truckPickupPercentageUnload, ContainerSpace targetContainerSpaceUnload) : base(placeName, shipSpaces, shipType)
    {
        if (unloadingCranes < shipSpaces)
        {
            throw new InvalidAmountOfCranesPerSpacesException("The amount of cranes can't be less than the amount of spaces");
        }
        if (targetContainerSpaceUnload == null)
        {
            throw new InvalidDestinationException("TargetContainerSpace cannot be null");
        }

        Cranes = unloadingCranes;
        TruckPickupPercentage = truckPickupPercentageUnload;
        TargetContainerSpace = targetContainerSpaceUnload;
    }

    /// <summary>
    /// Losser av containere fra skipene
    /// </summary>
    /// <param name="currentDateTime"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    internal int UnloadContainer(DateTime currentDateTime, DateTime end)
    {
        Random random = new Random();
        DateTime start = currentDateTime;
        var totalUnloadTime = 0;
        int TrucksDispatched = 0;
        foreach (var ship in new List<Ship>(Ships))
        {
            ship.Status = Status.Busy;
            int ContainersToUnload = ship.Containers.Count;
            int truckContainers = (int)(ContainersToUnload * TruckPickupPercentage / 100);
            int agvContainers = ContainersToUnload - truckContainers;

            while (ship.Containers.Count > 0 && start < end)
            {
                int TimePerContainer = Math.Max(1, 5 / Cranes);
                totalUnloadTime += TimePerContainer;
                start = start.AddMinutes(TimePerContainer);

                Container container = ship.MoveContainer();
                container.HistoriesInternal.Add(new HistoryService("Container " + container.Id, start, Name));
                ContainerHistory.Add(new HistoryService("Container " + container.Id, start, Name));

                if (truckContainers > 0)
                {
                    TrucksDispatched++;
                    truckContainers--;
                    start = start.AddMinutes(1);
                }
                else if (agvContainers > 0)
                {
                    AutomatedGuidedVehicle agv = TargetContainerSpace.AGVs.FirstOrDefault(a => a.Status == Status.Available);
                    if (agv != null)
                    {
                        agv.Container = container;
                        agv.Status = Status.Busy;

                        start = start.AddMinutes(1);

                        StorageColumn storageColumn = TargetContainerSpace.StorageColumns[random.Next(TargetContainerSpace.StorageColumns.Count)];
                        Column column = storageColumn.Columns[random.Next(storageColumn.Columns.Count)];

                        column.AddContainer(agv.Container);
                        container.HistoriesInternal.Add(new HistoryService("Container " + container.Id, start, Name + " StorageColumn"));
                        ContainerHistory.Add(new HistoryService("Container " + container.Id, start, Name + " StorageColumn"));
                        agv.Container = null;
                        agv.Status = Status.Available;
                        agvContainers--;
                    }
                }
                ContainersToUnload--;
            }
            if (ship.Repeat == false)
            {
                ship.Status = Status.Finished;
                Finished.Add(ship);
                Ships.Remove(ship);
            }
            else
                ship.Status = Status.Available;
        }
        return totalUnloadTime;
    }

}