using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    internal class Column
    {
        private readonly ContainerSpace containerSpace;

        internal Collection<Stack<Container>> StackedContainers;
        internal int AmountContainer {get; private set; }
        internal int MaxContainers;
        internal int MaxHeight;
        internal ContainerType Type = ContainerType.NONE;

        internal Column(int width, int height)
        {
            MaxContainers = width*height;
            MaxHeight = height;
            StackedContainers = new Collection<Stack<Container>>();
        }

        /// <summary>
        /// Legger til container i neste ledig plass og initialisere ContainerType for denne kolonnen da første container blir plassert
        /// </summary>
        /// <param name="container"></param>
        internal void AddContainer(Container container)
        {
            if(Type == ContainerType.NONE)
                InitializeContainerType(container);

            foreach (Stack<Container> stack in StackedContainers)
            {
                if(stack.Count < MaxHeight){
                    stack.Push(container);
                    break;
                }
            }
        }

        /// <summary>
        /// Initialisere ContainerType i kolonnen basert på containerens ContainerType
        /// </summary>
        /// <param name="container"></param>
        internal void InitializeContainerType(Container container)
        {
            if (container.Type == ContainerType.LONG)
            {
                for (int i = 0; i < MaxContainers/MaxHeight; i++)
                {
                    StackedContainers.Add(new Stack<Container>());
                }
            }
            else if (container.Type == ContainerType.SHORT)
            {
                MaxContainers = MaxContainers*2;
                for (int i = 0; i < (MaxContainers/MaxHeight)*2; i++)
                {
                    StackedContainers.Add(new Stack<Container>());
                }
            }

        Type = container.Type;
        }

        
        internal bool IsContainerLongOverdue(DateTime current)
        {
            foreach (Stack<Container> stack in StackedContainers)
            {
                foreach (Container container in stack)
                {
                    if ((current - container.Histories.Last().Time).TotalDays >= containerSpace.DaysInStorageLimit - 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal Container RetrieveOverdueContainer(DateTime current)
        {
            foreach (Stack<Container> stack in StackedContainers)
            {
                var temporaryStack = new Stack<Container>();
                Container overdueContainer = null;

                while (stack.Any())
                {
                    Container container = stack.Pop();
                    if (overdueContainer == null && (current - container.Histories.Last().Time).TotalDays >= containerSpace.DaysInStorageLimit - 1)
                    {
                        overdueContainer = container;
                        break;
                    }
                    else
                    {
                        temporaryStack.Push(container);
                    }
                }
                while (temporaryStack.Any())
                {
                    stack.Push(temporaryStack.Pop());
                }

                if (overdueContainer != null)
                {
                    return overdueContainer;
                }
            }
            return null;
        }
    }
}
