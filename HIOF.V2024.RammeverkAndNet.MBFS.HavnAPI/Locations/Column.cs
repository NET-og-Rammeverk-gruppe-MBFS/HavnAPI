using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    internal class Column
    {
        internal Collection<Stack<Container>> stackedContainers;
        internal int amountContainer {get; private set; }
        internal int maxContainers;
        internal int maxHeight;
        internal ContainerType Type = ContainerType.None;

        /// <summary>
        /// For å lage en kolonne som inneholder containere
        /// </summary>
        /// <param name="width"> Det er bredden basert på antall containere</param>
        /// <param name="height">Det er høyden basert på antall containere</param>
        internal Column(int width, int height)
        {
            maxContainers = width*height;
            maxHeight = height;
            stackedContainers = new Collection<Stack<Container>>();
        }

        /// <summary>
        /// Legger til container i neste ledig plass og initialisere ContainerType for denne kolonnen da første container blir plassert
        /// </summary>
        /// <param name="container"> container som skal bli lagt til kolonnen</param>
        internal void AddContainer(Container container)
        {
            if(Type == ContainerType.None)
                InitializeContainerType(container);

            foreach (Stack<Container> stack in stackedContainers)
            {
                if(stack.Count < maxHeight){
                    stack.Push(container);
                    break;
                }
            }
        }

        /// <summary>
        /// Initialisere ContainerType i kolonnen basert på containerens ContainerType
        /// </summary>
        /// <param name="container"> containeren som blir brukt for å initialisere ContainerType</param>
        internal void InitializeContainerType(Container container)
        {
            if (container.type == ContainerType.Long)
            {
                for (int i = 0; i < maxContainers/maxHeight; i++)
                {
                    stackedContainers.Add(new Stack<Container>());
                }
            }
            else if (container.type == ContainerType.Short)
            {
                maxContainers = maxContainers*2;
                for (int i = 0; i < (maxContainers/maxHeight)*2; i++)
                {
                    stackedContainers.Add(new Stack<Container>());
                }
            }

        Type = container.type;
        }

        /// <summary>
        /// Metode som ser om en av containerene ha nådd maks antall dager den kan være i kolonnen
        /// </summary>
        /// <param name="current">Nåværende tid</param>
        /// <param name="daysInStorageLimit">Maks antall dager</param>
        /// <returns> Returnerer boolean om det er noen containere som har nådd maks dager eller ikke</returns>
        internal bool IsContainerLongOverdue(DateTime current, int daysInStorageLimit)
        {
            foreach (Stack<Container> stack in stackedContainers)
            {
                foreach (Container container in stack)
                {
                    if ((current - container.histories.Last().time).TotalDays >= daysInStorageLimit - 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Metode som henter en container som har nådd maks antall dager den kan være i kolonnen
        /// </summary>
        /// <param name="current">Nåværende tid</param>
        /// <param name="daysInStorageLimit">Maks antall dager</param>
        /// <returns>Returnerer containeren og fjerner det fra kolonnen</returns>
        internal Container RetrieveOverdueContainer(DateTime current, int daysInStorageLimit)
        {
            foreach (Stack<Container> stack in stackedContainers)
            {
                foreach (Container container in stack)
                {
                    if ((current - container.histories.Last().time).TotalDays >= 1-daysInStorageLimit)
                    {
                        return stack.Pop();
                    }
                }
                if (stack.Count != 0)
                    return stack.Pop();
            }
            return null;
        }
    }
}
