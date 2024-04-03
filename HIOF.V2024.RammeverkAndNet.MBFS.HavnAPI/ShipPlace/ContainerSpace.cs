using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    public class ContainerSpace
    {
        internal Collection<StorageColumn> StorageColumns { get; private set; }
        internal Collection<AGV> AGVs { get; private set; }
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

        /// <summary>
        /// Legger til lagringskolonner og kolonner i hver lagringskolonne
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="numberOfCranes"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void AddStorageColumn(int amount, int numberOfCranes, int length, int width, int height)
        {
            for (int id = 1; id <= amount; id++)
            {
                StorageColumn newColumn = new StorageColumn(numberOfCranes, id, width, height);
                StorageColumns.Add(newColumn);
            }
        }

        /// <summary>
        /// Fjerner de containere som er lagret for lenge og fjerner dem fra lageret
        /// </summary>
        /// <param name="currentDate"></param>
        public DateTime OverdueContainers(DateTime currentDate) 
        {
            Stack<Container> overdueContainers = new Stack<Container>();
            int totalRemoveContainerTime = 0;
            int truckContainers = 0;
            int agvContainers = 0;
            DateTime start = currentDate;

            foreach (var storageColumn in StorageColumns)
            {
                foreach (var column in storageColumn.Columns)
                {
                    if (column.IsContainerLongOverdue(currentDate))
                    {
                        Container removedContainer;
                        while ((removedContainer = column.RetrieveOverdueContainer(currentDate)) != null)
                        {
                            overdueContainers.Push(removedContainer);
                        } 
                    }
                }
            }
            truckContainers = (int)(overdueContainers.Count * TruckPickupPercentage / 100);
            agvContainers = overdueContainers.Count - truckContainers;
            while (overdueContainers.Count > 0)
            {
                if (truckContainers > 0)
                {
                    overdueContainers.Pop();
                    truckContainers--;
                    start = start.AddMinutes(1);
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

                            overdueContainers.Pop();
                        }
                    }
                }
                start = start.AddMinutes(totalRemoveContainerTime);
            }
            return start; 
        }
    }
}
