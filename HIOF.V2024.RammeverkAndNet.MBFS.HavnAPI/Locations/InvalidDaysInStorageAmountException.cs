namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    /// <summary>
    /// Kaster et unntak når det er ugyldig antall dager
    /// </summary>
    public class InvalidDaysInStorageAmountException : Exception
    {
        public InvalidDaysInStorageAmountException() : base()
        {
        }
        
        public InvalidDaysInStorageAmountException(string message) : base(message)
        {
        }

        public InvalidDaysInStorageAmountException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
