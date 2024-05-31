namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships
{
    /// <summary>
    /// Kaster et unntak når destinasjonen sin shipstype er ugyldig
    /// </summary>
    public class InvalidShipTypeDestinationException : Exception
    {
        public InvalidShipTypeDestinationException() : base()
        {
        }
        
        public InvalidShipTypeDestinationException(string message) : base(message)
        {
        }

        public InvalidShipTypeDestinationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
