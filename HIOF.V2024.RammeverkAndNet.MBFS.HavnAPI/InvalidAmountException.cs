namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI

{
    /// <summary>
    /// Kaster et unntak når det er oppgitt ugyldig mengde
    /// </summary>
    public class InvalidAmountException : ArgumentOutOfRangeException
    {
        public InvalidAmountException() : base()
        {
        }
        
        public InvalidAmountException(string message) : base(message)
        {
        }

        public InvalidAmountException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
