using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
    public class MidnightStatusUpdateArgs : EventArgs
    {
        public IReadOnlyCollection<Ship> ShipList { get; private set; }

        /// <summary>
        /// Status for hver skip når det er midnatt
        /// </summary>
        public MidnightStatusUpdateArgs(IReadOnlyCollection<Ship> theShips)
        {
            ShipList = theShips;
        }
    }
}

