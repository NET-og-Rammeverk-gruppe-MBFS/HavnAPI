using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Simulation;
namespace HIOF.V2024.RammeverkAndNet.MBFS.TestClient;
class Historikk_test
{
    static void Main(string[] args)
    {
        // Lag 3 kaiplasser (dockspace), 2 ankerplasser (anchorage), og 2 losseplasser (unloadingspace).
        Dockspace kaiplass1 = new Dockspace("Liten_kaiplass", 2);
        Dockspace kaiplass2 = new Dockspace("Middels_kaiplass", 4);
        Dockspace kaiplass3 = new Dockspace("Stor_kaiplass", 6);

        Unloadingspace losseplass1 = new Unloadingspace("Liten_losseplass", 1, 50, 5);
        Unloadingspace losseplass2 = new Unloadingspace("Stor_losseplass", 5, 80, 10);

        List<ShipPlaces> shipPlaces = new List<ShipPlaces>();
        shipPlaces.Add(kaiplass1);
        shipPlaces.Add(kaiplass2);
        shipPlaces.Add(kaiplass3);
        shipPlaces.Add(losseplass1);
        shipPlaces.Add(losseplass2);

        // Lag bestemt antall skip objekter og knytt de til kaiplasser og losseplasser du lagde tidligere.
        Ship ship1 = new Ship("kaiplass1", kaiplass1, DateTime.Now, false, 0);
        Ship ship2 = new Ship("kaiplass2", kaiplass2, DateTime.Now, false, 0);
        Ship ship3 = new Ship("kaiplass3", losseplass2, DateTime.Now, true, 15);
        Ship ship4 = new Ship("losseplass1", losseplass1, DateTime.Now, false, 20);
        Ship ship5 = new Ship("losseplass1", losseplass1, DateTime.Now, false, 40);

        List<Ship> ships = new List<Ship>();
        ships.Add(ship1);
        ships.Add(ship2);
        ships.Add(ship3);
        ships.Add(ship4);
        ships.Add(ship5);

        // Lag en havn og legg til skipene og plassene du lagde tidligere.
        Harbour havn1 = new Harbour(ships, shipPlaces, "havn1", 50);


        havn1.ArrivedToHarbour += havn1_ArrivedToHarbour;
        havn1.DepartingAnchorage += havn1_DepartingAnchorage;
        havn1.MidnightStatusUpdate += havn1_MidnightStatusUpdate;
        havn1.MovingToAnchorage += havn1_MovingToAnchorage;
        havn1.ReachedDestination += havn1_ReachedDestination;
        havn1.ShipSailing += havn1_ShipSailing;

        havn1.Run(DateTime.Now, DateTime.Now.AddDays(3));

      /*  // Kjør run metoden til havn objektet, og hent ut all historikk etter slutten av simulasjonen.
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
        }*/
    }

    private static void havn1_ArrivedToHarbour(object? sender, ArrivedToHarbourArgs e)
    {
        Console.WriteLine(e.ship.ShipName+" har nådd havnen med "+e.ship.AmountContainers+" containere");
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

