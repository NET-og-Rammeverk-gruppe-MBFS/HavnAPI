namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI

{
    /// <summary>
    /// Kaster et unntak når det er oppgitt ugyldig mengde
    /// </summary>
    public class InvalidAmountException : ArgumentOutOfRangeException
    {
        public InvalidAmountException(string message) : base(message)
        {
        }
    }
}
