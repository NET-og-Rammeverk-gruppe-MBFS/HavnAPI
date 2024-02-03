using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;

public class Unloadingspace : ShipPlaces
{
    private int containerSpace { get; set; }
    private List<Container> containers { get; }

    public Unloadingspace(string Name, int Spaces, int containerSpaces) : base(Name, Spaces)
    {
        containers = new List<Container>();
        containerSpace = containerSpaces;
    }

    /// <summary>
    /// Metode for � legge til en container i losseplassen
    /// <summary>
    /// <param name="container">Containeren som skal legges til</param>
    public void AddContainer(Container container)
    {
    }


}