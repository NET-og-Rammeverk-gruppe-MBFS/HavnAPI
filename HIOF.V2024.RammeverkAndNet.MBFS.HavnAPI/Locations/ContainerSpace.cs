using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    public class ContainerSpace
    {
        /// <summary>
        /// Navnet til containerspace
        /// </summary>
        public string Name { get; private set; }
        internal Collection<StorageColumn> StorageColumns { get; private set; }
        internal Collection<AGV> AGVs { get; private set; }
        internal double TruckPickupPercentage { get; private set; }
        internal int DaysInStorageLimit { get; private set; }

        /// <summary>
        /// For å lage en containerspace.
        /// </summary>
        /// <param name="name"> Navn til containerspace-en</param>
        /// <param name="numberOfAGVs">Antall AGV kjøretøy i denne plassen</param>
        /// <param name="daysInStorageLimit">Maks antall dager en container kan lagres i denne plassen</param>
        /// <param name="truckPickupPercentage"> En prosentandel av lastebiler som kjører containere ut av plassen (Resten blir fraktet av et skip)</param>
        /// <exception cref="InvalidNameException"> Hvis du gir ugyldig navn som f.eks om det er tomt</exception>
        /// <exception cref="InvalidAmountException">Error for hvis du legger til ugyldig antall AGV-er</exception>
        /// <exception cref="InvalidDaysInStorageAmountException">Ugyldig antall dager</exception>
        /// <exception cref="InvalidPercentageExcpetion">Ugyldig bruk av prosent</exception>
        public ContainerSpace(String name, int numberOfAGVs, int daysInStorageLimit, double truckPickupPercentage)
        {
            if (string.IsNullOrWhiteSpace(name))
		    {
			    throw new InvalidNameException("name cannot be empty");
		    }
            if (numberOfAGVs < 1)
            {
                throw new InvalidAmountException("You need at least one AGV");
            }
            if (daysInStorageLimit < 1)
            {
                throw new InvalidDaysInStorageAmountException("Days in storage must be at least 1");
            }
            if (truckPickupPercentage < 0 || truckPickupPercentage > 1)
            {
                throw new InvalidPercentageExcpetion("The percentage can't be higher than 1 or less than 0");
            }
            Name = name;
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
        /// <param name="amount">Antall lagringskolonner</param>
        /// <param name="numberOfCranes"> Antall kraner for denne lagringskolonnen</param>
        /// <param name="length">Det er lengden basert på antall containere</param>
        /// <param name="width">Det er bredden basert på antall containere</param>
        /// <param name="height">Det er høyden basert på antall containere</param>
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
        internal int OverdueContainers(DateTime currentDate, DateTime end) 
        {
            Stack<Container> overdueContainers = new Stack<Container>();
            int totalRemoveContainerTime = 0;
            int truckContainers = 0;
            int agvContainers = 0;
            int time = 0;
            DateTime start = currentDate;

            foreach (var storageColumn in StorageColumns)
            {
                foreach (var column in storageColumn.Columns)
                {
                    if (column.IsContainerLongOverdue(currentDate, DaysInStorageLimit))
                    {
                        Container removedContainer;
                        while ((removedContainer = column.RetrieveOverdueContainer(currentDate, DaysInStorageLimit)) != null)
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
                    time += 1;
                }

                else if (agvContainers > 0)
                {
                    foreach (var agv in AGVs)
                    {
                        if (agvContainers > 0 && agv.status == Status.Available)
                        {
                            agv.status = Status.Busy;
                            start = start.AddMinutes(1);
                            time += 1;
                        
                            agv.status = Status.Available;
                            agvContainers--;

                            overdueContainers.Pop();
                        }
                    }
                }
                start = start.AddMinutes(totalRemoveContainerTime);
            }
            return time; 
        }
    }
}
