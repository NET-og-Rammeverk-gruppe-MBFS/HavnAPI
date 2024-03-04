using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class Shelf 
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public GoodsType GoodsType { get; set; }
        public string Area { get; set; }
        public int TerminalToShelfTime { get; set; }
        public int ShelfToTerminalTime { get; set; } 
        public List<Item> Items { get; set; }

        public Shelf(string id, GoodsType goodsType, int capacity){ }
        public void AddItem(Item item){}

        public void RemoveItem(Item item) {}

        public static Shelf CreateShelf(string id, GoodsType goodsType, int capacity)
        {
            return new Shelf(id, goodsType, capacity);
        }
        public static void AddShelfToWarehouse(List<Shelf> warehouse, Shelf shelf){}
        public static void RemoveShelfFromWarehouse(List<Shelf> warehouse, string id){}
    }
}
