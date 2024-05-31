namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    /// <summary>
    /// Kaster et unntak når det er mindre kraner enn plasser(skip-plasser)
    /// </summary>
    public class InvalidAmountOfCranesPerSpacesException : Exception
    {
        public InvalidAmountOfCranesPerSpacesException() : base()
        {
        }

        public InvalidAmountOfCranesPerSpacesException(string message) : base(message)
        {
        }

        public InvalidAmountOfCranesPerSpacesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
