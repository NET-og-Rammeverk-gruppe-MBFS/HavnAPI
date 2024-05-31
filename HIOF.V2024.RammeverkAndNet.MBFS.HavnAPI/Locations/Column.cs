using System.Collections.ObjectModel;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    internal class Column
    {
        internal Collection<Stack<Container>> StackedContainers;
        internal int AmountContainer {get; private set; }
        internal int MaxContainers;
        internal int MaxHeight;
        internal ContainerType Type = ContainerType.None;

        internal Column(int width, int height)
        {
            MaxContainers = width*height;
            MaxHeight = height;
            StackedContainers = new Collection<Stack<Container>>();
        }

        internal void AddContainer(Container container)
        {
            if(Type == ContainerType.None)
                InitializeContainerType(container);

            foreach (Stack<Container> stack in StackedContainers)
            {
                if(stack.Count < MaxHeight){
                    stack.Push(container);
                    break;
                }
            }
        }

        internal void InitializeContainerType(Container container)
        {
            if (container.Type == ContainerType.Long)
            {
                for (int i = 0; i < MaxContainers/MaxHeight; i++)
                {
                    StackedContainers.Add(new Stack<Container>());
                }
            }

            else if (container.Type == ContainerType.Short)
            {
                MaxContainers = MaxContainers*2;
                for (int i = 0; i < (MaxContainers/MaxHeight)*2; i++)
                {
                    StackedContainers.Add(new Stack<Container>());
                }
            }

        Type = container.Type;
        }

        internal bool IsContainerLongOverdue(DateTime current, int daysInStorageLimit)
        {
            foreach (Stack<Container> stack in StackedContainers)
            {
                foreach (Container container in stack)
                {
                    if ((current - container.HistoriesInternal.Last().Time).TotalDays >= daysInStorageLimit - 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal Container RetrieveOverdueContainer(DateTime current, int daysInStorageLimit)
        {
            foreach (Stack<Container> stack in StackedContainers)
            {
                foreach (Container container in stack)
                {
                    if ((current - container.HistoriesInternal.Last().Time).TotalDays >= 1-daysInStorageLimit)
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
