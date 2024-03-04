using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class Pickup
    {
        public int SimulationDay { get; set; }
        public GoodsType GoodsType { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; }

        public Pickup(){}

        public Pickup(int simulationDay, GoodsType goodsType, int quantity, Item item)
        {
            SimulationDay = simulationDay;
            GoodsType = goodsType;
            Quantity = quantity;
            Item = item;
        }

        public DateTime GetPickupTime(DateTime dateTime)
        {
            return default(DateTime);
        }
        public void SchedulePickup(Warehouse warehouse){}
        public void ScheduleWeeklyPickup(Warehouse warehouse, int dayOfWeek){}
        public void Process(Warehouse warehouse){}
        public void ItemLeftWarehouse(Item item){}
    }
}
