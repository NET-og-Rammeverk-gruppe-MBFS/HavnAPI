using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class DeliverySchedule
    {
        public List<Delivery> deliveries { get; set; }

        public void addDelivery(Delivery delivery) { }
        public void addWeeklyDelivery(Delivery delivery, int dayOfWeek) { }
        public List<Delivery> Deliveries() { return null; }
    }
}
