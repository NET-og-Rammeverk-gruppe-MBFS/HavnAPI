using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
    public class MovingToAnchorageArgs :EventArgs
    {
        public Ship TheShip { get; set; }

        /// <summary>
        /// Lar skip seile til ankerplassen dersom destinasjonen er full
        /// </summary>
        public MovingToAnchorageArgs(Ship theShip)
        {
           TheShip = theShip;
        }
    }
}

