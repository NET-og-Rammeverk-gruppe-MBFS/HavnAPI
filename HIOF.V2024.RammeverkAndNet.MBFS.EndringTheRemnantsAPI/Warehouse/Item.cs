using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class Item
    {
        public string Name { get; set; }
        public GoodsType GoodsType { get; set; }
        public List<string> LocationHistory { get; set; }


        public Item(string name, GoodsType goodsType)
        {
            Name = name;
            GoodsType = goodsType;
            LocationHistory = new List<string>();
        }
        public void UpdateLocationHistory(string location){}
        public void PrintHistory(){}
        public static Item FindItemByName(List<Item> items, string name)
        {return null;}
        public static void PrintItemHistory(List<Item> items, string name){}
    }
}
