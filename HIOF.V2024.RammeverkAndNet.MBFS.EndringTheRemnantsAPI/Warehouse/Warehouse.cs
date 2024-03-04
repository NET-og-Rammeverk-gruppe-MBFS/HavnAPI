using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class Warehouse
    {
        public List<Shelf> Shelves { get; set; }
        public WarehouseConfiguration Configuration { get; set; }
        public DeliverySchedule DeliverySchedule { get; set; }
        public PickupSchedule PickupSchedule { get; set; }
        public Terminal Terminal { get; set; }
        public int CurrentSimulationDay { get; set; }
        public List<Item> ItemHistory { get; set; }
        public Simulation Simulation { get; set; }
        public Warehouse(WarehouseConfiguration configuration){}


        public void AddItem(string id, GoodsType type){}
        public void RemoveShelf(string id){}
        public void AddShelf(string id, GoodsType type, int capacity){}
        public void AddDelivery(int day, GoodsType type, string id){}
        public void AddWeeklyDelivery(int day, GoodsType type, string id){}
        public void AddPickup(int day, GoodsType type, string id){}
        public void ProcessDelivery(Delivery delivery){}
        public void ProcessPickup(Pickup pickup){}
        public void PrintItemHistory(string id){}
        public void SimulationRun(int days) {}
        public Item FindOrCreateItem(string id, GoodsType type) {return null;}
    }
}
