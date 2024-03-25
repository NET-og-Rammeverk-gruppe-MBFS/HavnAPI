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
        internal Collection<Stack<Container>> StackedContainers;
        public int AmountContainer {get; private set; }
        internal int MaxContainers;
        internal int MaxHeight;
        internal ContainerType Type = ContainerType.NONE;

        internal Column(int width, int height)
        {
            MaxContainers = width*height;
            MaxHeight = height;
            StackedContainers = new Collection<Stack<Container>>();
        }

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

        internal void InitializeContainerType(Container container)
        {
            if (container.Type == ContainerType.LONG)
            {
                for (int i = 0; i < MaxContainers/MaxHeight; i++)
                {
                    StackedContainers.Add(new Stack<Container>());
                }
            }
            else if (container.Type == ContainerType.LONG)
            {
                MaxContainers = MaxContainers*2;
                for (int i = 0; i < (MaxContainers/MaxHeight)*2; i++)
                {
                    StackedContainers.Add(new Stack<Container>());
                }
            }

        Type = container.Type;
        }
    }
}
