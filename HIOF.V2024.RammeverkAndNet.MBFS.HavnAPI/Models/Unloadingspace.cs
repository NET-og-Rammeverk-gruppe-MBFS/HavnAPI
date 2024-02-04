using System.Runtime.ExceptionServices;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;

public class Unloadingspace : ShipPlaces
{
    private int containerSpace { get; set; }
    private int RemoveFrequency { get; set; }
    private List<Container> containers { get; }

    public Unloadingspace(string Name, int Spaces, int containerSpaces, int emptyFrequency) : base(Name, Spaces)
    {
        containers = new List<Container>();
        containerSpace = containerSpaces;
        RemoveFrequency = emptyFrequency;

    }

    /// <summary>
    /// Metode for � legge til en container i losseplassen
    /// <summary>
    /// <param name="container">Containeren som skal legges til</param>
    public int UnloadContainer(DateTime currentDateTime)
    {
        DateTime start = currentDateTime;
        var timer = 0;
        var timerRemoval = 0;
        foreach (var ship in Ships)
        {
            foreach (var Thecontainer in ship.containers)
            {
                if (containerSpace != 0)
                {
                    timer += 5;
                    timerRemoval += 5;
                    start.AddMinutes(5);
                    Container container = ship.MoveContainer();
                    container.Histories.Add(new HistoryService(Name, start));
                    containers.Add(container);
                    containerSpace--;
                    if (containers.Count > 0 && RemoveFrequency <= timerRemoval)
                    {
                        for (int i = 0; i < timerRemoval / RemoveFrequency; i++)
                        {
                            containers.RemoveAt(-1);
                        }
                        timerRemoval = 0;
                    }
                }
                else
                {
                    throw new Exception("No space for container");
                }
            }

        }
        return timer;
    }

    public List<Ship> ReturnShips()
    {
        List < Ship > OldShips = new List<Ship>(Ships);
        Ships.Clear();
        return OldShips;
    }
}