using System;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI
{
    /// <summary>
    /// Type som beskriver hva slags type skip blir brukt eller er tillatt
    /// </summary>
    public enum ShipType
    {
        /// <summary>
        /// Denne typen brukes hvis skipet blir brukt for alle situasjoner eller hvis et plass godtar alle typer skip
        /// </summary>
        All,
        /// <summary>
        /// Cargo er en type som beskriver at skipet har containere
        /// </summary>
        Cargo,
        /// <summary>
        /// Passenger beskriver at skipet blir brukt kun for å seile personer
        /// </summary>
        Passenger,
        /// <summary>
        /// Tankship beskriver at det er gass og andre farlig stoffer i skipet
        /// </summary>
        Tankship
    }
}

