using System.Runtime.ExceptionServices;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Unloadingspace : ShipPlaces
{
    private int containerSpace { get; set; }
    private int RemoveFrequency { get; set; }
    private Queue<Container> TempContainers { get; }
    internal List<Container> containerSaved { get; }

    public Unloadingspace(string Name, int Spaces, int containerSpaces, int emptyFrequency) : base(Name, Spaces)
    {
        containerSaved = new List<Container>();
        TempContainers = new Queue<Container>();
        containerSpace = containerSpaces;
        RemoveFrequency = emptyFrequency;

    }

    /// <summary>
    /// Metoden legger til en container i losseplassen fra shipene som er i Ship listen, og fjerne containers fra plassene basert på emptyFrequency
    /// <summary>
    /// <param name="currentDateTime"> Det blir brukt for å lagre tiden i historikken til en container objekt under simulasjonen</param>
    internal int UnloadContainer(DateTime currentDateTime, DateTime end)
    {
        DateTime start = currentDateTime;
        var timer = 0;
        var timerRemoval = 0;
        foreach (var ship in new List<Ship>(Ships))
        {
            foreach (var Thecontainer in new Queue<Container>(ship.containers))
            {
                if (containerSpace != 0)
                {
                    timer += 5;
                    timerRemoval += 5;
                    start = start.AddMinutes(5);
                    Container container = ship.MoveContainer();
                    container.Histories.Add(new HistoryService(Name, start));
                    TempContainers.Enqueue(container);
                    if (TempContainers.Count > 0 && RemoveFrequency >= timerRemoval)
                    {
                        for (int i = 0; i < RemoveFrequency / timerRemoval; i++)
                        {
                            if(TempContainers.Count != 0)
                            {
                                containerSaved.Add(TempContainers.Dequeue());
                            }
                        }
                        timerRemoval = 0;
                    }
                    else
                    {
                        throw new InvalidFrequencyException("EmptyFrequency is slow");
                    }

                    if (start >= end)
                        break; 
                }
            }
            if(ship.Repeat == false)
            {
                Finished.Add(ship);
                Ships.Remove(ship);
            }

        }
        return timer;
    }

    internal override void AddShip(Ship ship)
    {
        Ships.Add(ship);
    }


}