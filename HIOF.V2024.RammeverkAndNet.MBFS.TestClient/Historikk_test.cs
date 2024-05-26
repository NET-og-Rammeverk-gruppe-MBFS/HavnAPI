using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulations;
namespace HIOF.V2024.RammeverkAndNet.MBFS.TestClient;
class Historikk_test
{
    static void Main(string[] args)
    {
        ContainerSpace ContainerZone1 = new ContainerSpace("arle",20 , 4, 0.10);
        ContainerZone1.AddStorageColumn(24, 1, 18, 6, 4);
        ContainerZone1.AddStorageColumn(7, 1, 15, 6, 4);
        
        Dockspace kaiplass1 = new Dockspace("Liten_kaiplass", 2, HavnAPI.ShipType.Passenger);
        Dockspace kaiplass2 = new Dockspace("Middels_kaiplass", 4, HavnAPI.ShipType.Passenger);
        Dockspace kaiplass3 = new Dockspace("Stor_kaiplass", 6, HavnAPI.ShipType.Passenger);

        Unloadingspace losseplass1 = new Unloadingspace("Liten_losseplass", 1, HavnAPI.ShipType.Cargo,1, 0.5, ContainerZone1);
        Unloadingspace losseplass2 = new Unloadingspace("Stor_losseplass", 5, HavnAPI.ShipType.Cargo, 5, 0.5, ContainerZone1);

        List<ShipPlaces> shipPlaces = new List<ShipPlaces>();
        shipPlaces.Add(kaiplass1);
        shipPlaces.Add(kaiplass2);
        shipPlaces.Add(kaiplass3);
        shipPlaces.Add(losseplass1);
        shipPlaces.Add(losseplass2);

        
        Ship ship1 = new Ship("Bob", kaiplass1, DateTime.Now, 0, 0, HavnAPI.ShipType.Passenger);
        Ship ship2 = new Ship("Fred", kaiplass2, DateTime.Now, 0, 0, HavnAPI.ShipType.Passenger);
        Ship ship3 = new Ship("Ibrahim", losseplass2, DayOfWeek.Monday, 10, 10, HavnAPI.ShipType.Cargo);
        Ship ship4 = new Ship("Magnus", losseplass1, TimeOnly.Parse("15:00"), 20, 0, HavnAPI.ShipType.Cargo);
        Ship ship5 = new Ship("Colorline", losseplass1, TimeOnly.Parse("17:00"), 30, 10, HavnAPI.ShipType.Cargo);

        List<Ship> ships = new List<Ship>();
        ships.Add(ship1);
        ships.Add(ship2);
        ships.Add(ship3);
        ships.Add(ship4);
        ships.Add(ship5);

       
        Harbour havn1 = new Harbour("havn1", 50, ships, shipPlaces);


        havn1.ArrivedToHarbour += havn1_ArrivedToHarbour;
        havn1.DepartingAnchorage += havn1_DepartingAnchorage;
        havn1.MidnightStatusUpdate += havn1_MidnightStatusUpdate;
        havn1.MovingToAnchorage += havn1_MovingToAnchorage;
        havn1.ReachedDestination += havn1_ReachedDestination;
        havn1.ShipSailing += havn1_ShipSailing;

        havn1.Run(DateTime.Now, DateTime.Now.AddDays(5));

       
        Console.WriteLine("Results:");
        Console.WriteLine(" ");
        Console.WriteLine("Ship:");
       foreach (HistoryService ShipHistory in havn1.ShipHistory)
        {
            Console.WriteLine(ShipHistory.ToString());
        }
        Console.WriteLine(" ");
        Console.WriteLine("Container:");
        foreach (HistoryService ContainerHistory in havn1.ContainerHistory)
        {
            Console.WriteLine(ContainerHistory.ToString());
        }
    }

    private static void havn1_ArrivedToHarbour(object? sender, ArrivedToHarbourArgs e)
    {
        Console.WriteLine(e.TheShip.ShipName+" har nådd havnen med "+e.TheShip.TotalContainers+" containere");
    }

    private static void havn1_DepartingAnchorage(object? sender, DepartingAnchorageArgs e)
    {
        Console.WriteLine(e.TheShip.PlaceDestination.Name+" er ledig, "+e.TheShip.ShipName+" blir flyttet fra ankerplassen");
    }

    private static void havn1_MidnightStatusUpdate(object? sender, MidnightStatusUpdateArgs e)
    {
        Console.WriteLine("\n-----------------------------");
        Console.WriteLine("Midnatt status");
        foreach (Ship ship in e.ShipList)
        {
            Console.WriteLine("\n--------------");
            Console.WriteLine("Ship name: "+ship.ShipName);
            Console.WriteLine("Current location: "+ship.CurrentLocation);
            Console.WriteLine("Current status: "+ship.Status);
            Console.WriteLine("Targeted Destination: "+ship.PlaceDestination.Name);
            Console.WriteLine("--------------");
        }
        Console.WriteLine("-----------------------------");
    }

    private static void havn1_MovingToAnchorage(object? sender, MovingToAnchorageArgs e)
    {
        Console.WriteLine(e.TheShip.PlaceDestination.Name+" er full, flytter "+e.TheShip.ShipName+" til ankerplassen");
    }

    private static void havn1_ReachedDestination(object? sender, ReachedDestinationArgs e)
    {
        Console.WriteLine(e.TheShip.ShipName+" har nådd "+e.TheShip.PlaceDestination.Name);
    }

    private static void havn1_ShipSailing(object? sender, ShipSailingArgs e)
    {
        Console.WriteLine(e.TheShip.ShipName+ " seiler...");
    }
}

