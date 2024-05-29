using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI
{
    /// <summary>
    /// Status beskriver nåværende tilstand til objektet
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Available beskriver at objektet er ledig
        /// </summary>
        Available,
        /// <summary>
        /// Busy beskriver at objektet er opptatt
        /// </summary>
        Busy,
        /// <summary>
        /// Finished beskriver at objektet er ferdig med målet sitt
        /// </summary>
        Finished,
    }
}