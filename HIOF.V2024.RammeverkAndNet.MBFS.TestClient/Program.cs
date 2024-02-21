using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Services;
namespace HIOF.V2024.RammeverkAndNet.MBFS.TestClient;
class Program
{
    static void Main(string[] args)
    {
        Anchorage venteplass1 = new Anchorage("Ahmed", 5);
        Anchorage venteplass2 = new Anchorage("Ahmed", 5);
        Unloadingspace losseplass1 = new Unloadingspace("Arne", 10, 10,5);
        Unloadingspace losseplass2 = new Unloadingspace("Gunnar", 10, 10, 5);
        Dockspace kaiplass1 = new Dockspace("Bardh", 20);

        List<ShipPlaces> shipPlaces = new List<ShipPlaces>();
        shipPlaces.Add(venteplass1);
        shipPlaces.Add(venteplass2);
        shipPlaces.Add(losseplass1);
        shipPlaces.Add(kaiplass1);

        Ship ship1 = new Ship("Sadik", losseplass1, DateTime.Now, true, 15);
        Ship ship2 = new Ship("Bardh", kaiplass1, DateTime.Now, true, 15);
        Ship ship3 = new Ship("Fredrik", losseplass1, DateTime.Now, true, 15);
        Ship ship4 = new Ship("Mahamoud", kaiplass1, DateTime.Now, true, 15);

        List<Ship> ships = new List<Ship>();
        ships.Add(ship1);
        ships.Add(ship2);
        ships.Add(ship3);
        ships.Add(ship4);

        Harbour havn1 = new Harbour(ships, shipPlaces);

        havn1.Run(DateTime.Now, DateTime.Now.AddHours(1));

        Console.WriteLine("Results:");
        Console.WriteLine("Ship:");
        foreach (HistoryService ShipHistory in havn1.ShipHistory)
        {
            Console.WriteLine(ShipHistory.ToString());
        }
        Console.WriteLine("Container:");
        foreach (HistoryService ContainerHistory in havn1.ContainerHistory)
        {
            Console.WriteLine(ContainerHistory.ToString());
        }
    }
}

