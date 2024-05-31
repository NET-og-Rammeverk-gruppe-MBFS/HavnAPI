using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
    public class DepartingHarbourArgs : EventArgs
    {
        public Ship TheShip { get; private set; }
        /// <summary>
        /// Event der skipen forlater havnen
        /// </summary>
        public DepartingHarbourArgs(Ship theship)
        {
            TheShip = theship;
        }
    }
}

