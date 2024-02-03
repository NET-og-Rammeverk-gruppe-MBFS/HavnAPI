using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models

public class Unloadingspace : ShipPlaces
{
    public Unloadingspace(string Name, int Spaces, int containerSpace) : base(name, spaces)
    {
        containerSpace = new List<Container>();
        containerSpace = containerSpace;
    }

    /// <summary>
    /// Metode for å legge til en container i losseplassen
    /// <summary>
    /// <param name="container">Containeren som skal legges til</param>
    public void AddContainer(Container container)
    {
    }


}