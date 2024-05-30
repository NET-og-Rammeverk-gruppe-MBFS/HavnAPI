namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI

{
    /// <summary>
    /// Kaster et unntak når det er oppgitt et ugyldig navn
    /// </summary>
    public class InvalidNameException : ArgumentException
    {
        public InvalidNameException() : base()
        {
        }
        
        public InvalidNameException(string message) : base(message)
        {
        }

        public InvalidNameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
