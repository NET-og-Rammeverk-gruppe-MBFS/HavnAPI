using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulation;
namespace HIOF.V2024.RammeverkAndNet.MBFS.TestClient;
class Historikk_test
{
    static void Main(string[] args)
    {
        ContainerSpace ContainerZone1 = new ContainerSpace(20);
        ContainerZone1.AddStorageColumn(24, 1, 18, 6, 4);
        ContainerZone1.AddStorageColumn(7, 1, 15, 6, 4);
        
        Dockspace kaiplass1 = new Dockspace("Liten_kaiplass", 2, HavnAPI.ShipType.passenger);
        Dockspace kaiplass2 = new Dockspace("Middels_kaiplass", 4, HavnAPI.ShipType.passenger);
        Dockspace kaiplass3 = new Dockspace("Stor_kaiplass", 6, HavnAPI.ShipType.passenger);

        Unloadingspace losseplass1 = new Unloadingspace("Liten_losseplass", 1, HavnAPI.ShipType.cargo,1, 0.5, ContainerZone1);
        Unloadingspace losseplass2 = new Unloadingspace("Stor_losseplass", 5,HavnAPI.ShipType.cargo, 5, 0.5, ContainerZone1);

        List<ShipPlaces> shipPlaces = new List<ShipPlaces>();
        shipPlaces.Add(kaiplass1);
        shipPlaces.Add(kaiplass2);
        shipPlaces.Add(kaiplass3);
        shipPlaces.Add(losseplass1);
        shipPlaces.Add(losseplass2);

        
        Ship ship1 = new Ship("Bob", kaiplass1, DateTime.Now, false, 0, 0, HavnAPI.ShipType.passenger);
        Ship ship2 = new Ship("Fred", kaiplass2, DateTime.Now, false, 0, 0, HavnAPI.ShipType.passenger);
        Ship ship3 = new Ship("Ibrahim", losseplass2, DateTime.Now, false, 10, 10, HavnAPI.ShipType.cargo);
        Ship ship4 = new Ship("Magnus", losseplass1, DateTime.Now, false, 20, 0, HavnAPI.ShipType.cargo);
        Ship ship5 = new Ship("Colorline", losseplass1, DateTime.Now, false, 30, 10, HavnAPI.ShipType.cargo);

        List<Ship> ships = new List<Ship>();
        ships.Add(ship1);
        ships.Add(ship2);
        ships.Add(ship3);
        ships.Add(ship4);
        ships.Add(ship5);

       
        Harbour havn1 = new Harbour(ships, shipPlaces, "havn1", 50);


        havn1.ArrivedToHarbour += havn1_ArrivedToHarbour;
        havn1.DepartingAnchorage += havn1_DepartingAnchorage;
        havn1.MidnightStatusUpdate += havn1_MidnightStatusUpdate;
        havn1.MovingToAnchorage += havn1_MovingToAnchorage;
        havn1.ReachedDestination += havn1_ReachedDestination;
        havn1.ShipSailing += havn1_ShipSailing;

        havn1.Run(DateTime.Now, DateTime.Now.AddDays(2));

       
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
        Console.WriteLine(e.ship.ShipName+" har nådd havnen med "+e.ship.TotalContainers+" containere");
    }

    private static void havn1_DepartingAnchorage(object? sender, DepartingAnchorageArgs e)
    {
        Console.WriteLine(e.ship.PlaceDestination.Name+" er ledig, "+e.ship.ShipName+" blir flyttet fra ankerplassen");
    }

    private static void havn1_MidnightStatusUpdate(object? sender, MidnightStatusUpdateArgs e)
    {
        Console.WriteLine("\n-----------------------------");
        Console.WriteLine("Midnatt status");
        Console.WriteLine(e.ship.ShipName);
        Console.WriteLine("-----------------------------");
    }

    private static void havn1_MovingToAnchorage(object? sender, MovingToAnchorageArgs e)
    {
        Console.WriteLine(e.ship.PlaceDestination.Name+" er full, flytter "+e.ship.ShipName+" til ankerplassen");
    }

    private static void havn1_ReachedDestination(object? sender, ReachedDestinationArgs e)
    {
        Console.WriteLine(e.ship.ShipName+" har nådd "+e.ship.PlaceDestination.Name);
    }

    private static void havn1_ShipSailing(object? sender, ShipSailingArgs e)
    {
        Console.WriteLine(e.ship.ShipName+ " seiler...");
    }
}

