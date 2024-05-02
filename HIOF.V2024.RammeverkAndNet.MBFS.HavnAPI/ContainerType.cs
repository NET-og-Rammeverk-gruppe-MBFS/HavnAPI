namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI
{
    /// <summary>
    /// Type container som hver container har i følge ISO
    /// </summary>
    public enum ContainerType
    {
        /// <summary>
        /// None er en type som vil si at container ikke ennå har fått en type.
        /// </summary>
        None,
        /// <summary>
        /// Long er en type for lange ISO container
        /// </summary>
        Long,
        /// <summary>
        /// Short er en type for korte ISO container
        /// </summary>
        Short
    }
}