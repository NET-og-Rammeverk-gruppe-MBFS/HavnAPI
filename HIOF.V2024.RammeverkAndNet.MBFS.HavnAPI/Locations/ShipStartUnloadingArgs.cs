using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    public class ShipStartUnloadingArgs : EventArgs
    {
        public Ship TheShip { get; private set; }

        /// <summary>
        /// Når losseplassen begynner å losse containers
        /// </summary>
        public ShipStartUnloadingArgs(Ship theShip)
        {
            TheShip = theShip;
        }
    }
}

