using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class Terminal
    {
        public List<Item> Items { get; private set; }
        public int Capacity { get; private set; }

        public event Action<Item> ItemRemoved;

        public Terminal(int capacity){}
        public void AddItem(Item item){}

        public void RemoveItem(Item item){}

        public List<Item> GetItems()
        {
            return Items;
        }

        public void ConfigureTerminal(int newCapacity) {}
    }
}
