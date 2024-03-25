using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    public class ContainerSpace
    {
        internal Collection<StorageColumn> StorageColumns { get; private set; }
        public Collection<AGV> AGVs { get; private set; }
        internal double TruckPickupPercentage { get; private set; }
        internal int DaysInStorageLimit { get; private set; }

        public ContainerSpace(int numberOfAGVs, int daysInStorageLimit, double truckPickupPercentage)
        {
            TruckPickupPercentage = truckPickupPercentage;
            DaysInStorageLimit = daysInStorageLimit;
            AGVs = new Collection<AGV>();
            StorageColumns = new Collection<StorageColumn>();

            for (int i = 0; i < numberOfAGVs; i++)
            {
                AGVs.Add(new AGV());
            }
        }

        public void AddStorageColumn(int amount, int numberOfCranes, int length, int width, int height)
        {
            for (int id = 1; id <= amount; id++)
            {
                StorageColumn newColumn = new StorageColumn(numberOfCranes, id, width, height);
                StorageColumns.Add(newColumn);
            }
        }

        public void OverdueContainers(DateTime currentDate) 
        {
            List<Container> overdueContainers = new List<Container>();
            //Må lage en logikk for tiden
            int truckContainers = 0;
            int agvContainers = 0;
            int trucksDispatched = 0;
            DateTime start = currentDate;

            foreach (var storageColumn in StorageColumns)
            {
                foreach (var column in storageColumn.Columns)
                {
                    Container removedContainer;
                    while ((removedContainer = column.RetrieveOverdueContainer(currentDate)) != null)
                    {
                        overdueContainers.Add(removedContainer);
                    }
                }
            }
            truckContainers = (int)(overdueContainers.Count * TruckPickupPercentage / 100);
            agvContainers = overdueContainers.Count - truckContainers;

            if (truckContainers > 0)
            {
                start = start.AddSeconds(30);
                truckContainers--;
            }

            else if (agvContainers > 0)
            {
                foreach (var agv in AGVs)
                {
                    if (agvContainers > 0 && agv.status == Status.Available)
                    {
                        agv.status = Status.Busy;
                        start = start.AddMinutes(1);    
                        
                        agv.status = Status.Available;
                        agvContainers--;
                    }
                }
            }
        }
    }
}
