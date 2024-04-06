namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI
{
    /// <summary>
    /// Kaster et unntak når en metode ikke er implementert
    /// </summary>
    public class NotImplementedException : Exception
    {
        public NotImplementedException(string message) : base(message)
        {
        }
    }
}
