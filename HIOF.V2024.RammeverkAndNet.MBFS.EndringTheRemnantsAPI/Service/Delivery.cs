using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class Delivery
    {
        public int SimulationDay { get; set; }
        public GoodsType GoodsType { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; }

        public Delivery(int simulationDay, GoodsType goodsType, int quantity, Item item)
        {
            SimulationDay = simulationDay;
            GoodsType = goodsType;
            Quantity = quantity;
            Item = item;
        }
        public DateTime GetDeliveryTime(DateTime dateTime)
        {
            return default(DateTime);
        }
        public void ScheduleDelivery(Warehouse warehouse){}
        public void ScheduleWeeklyDelivery(Warehouse warehouse, int dayOfWeek){}
        public void Process(Warehouse warehouse){}

    }
}
