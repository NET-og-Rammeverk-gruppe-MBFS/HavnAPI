namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI
{
    /// <summary>
    /// Kaster et unntak når en metode ikke er implementert
    /// </summary>
    public class NotImplementedException : Exception
    {
        public NotImplementedException() : base()
        {
        }
        
        public NotImplementedException(string message) : base(message)
        {
        }

        public NotImplementedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
