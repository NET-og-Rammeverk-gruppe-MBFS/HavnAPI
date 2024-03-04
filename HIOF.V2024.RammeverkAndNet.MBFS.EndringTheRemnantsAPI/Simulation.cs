using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.EndringTheRemnantsAPI
{
    internal class Simulation
    {
        public int CurrentSimulationDay { get; set; }


        public void SimulateDay(Warehouse warehouse){}
        public void GenerateDailyShelfReport(Warehouse warehouse){}
        public void SimulationRun(Warehouse warehouse, int numberOfDays){}
    }
}
