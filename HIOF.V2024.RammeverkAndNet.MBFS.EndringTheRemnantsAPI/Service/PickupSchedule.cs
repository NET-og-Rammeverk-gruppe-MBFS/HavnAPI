using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class PickupSchedule
    {
        public List<Pickup> Pickups { get; set; }

        public void AddPickup(Pickup pickup)
        {
            Pickups.Add(pickup);
        }
        public List<Pickup> GetPickups()
        {
            return Pickups;
        }
        public void AddWeeklyPickup(Pickup pickup, int dayOfWeek){}

    }
}
