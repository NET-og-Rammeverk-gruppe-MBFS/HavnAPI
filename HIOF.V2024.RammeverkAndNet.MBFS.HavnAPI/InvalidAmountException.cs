namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI

{
    /// <summary>
    /// Kaster et unntak når det er oppgitt plasser utenfor det akseptable området
    /// </summary>
    public class InvalidSpacesException : ArgumentOutOfRangeException
    {
        public InvalidSpacesException(string message) : base(message)
        {
        }
    }
}
