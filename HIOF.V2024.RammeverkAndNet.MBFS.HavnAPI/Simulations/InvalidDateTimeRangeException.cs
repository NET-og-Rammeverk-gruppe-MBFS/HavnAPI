
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations
{
    /// <summary>
    /// Kaster et unntak når det er oppgitt et ugyldig tidsrom
    /// </summary>
    public class InvalidDateTimeRangeException : ArgumentException
    {
        public InvalidDateTimeRangeException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}
