using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Services;
public class Harbour : IHarbour
{
    private List<HistoryService> ShipHistory;
    private List<HistoryService> ContainerHistory;
    private List<ShipPlaces> ShipPlacesList;
    private List<Ship> ShipsList;

    public Harbour(List<Ship> ships, List<ShipPlaces> shipPlaces)
    {
        ShipsList = new List<Ship>(ships);
        ShipPlacesList = new List<ShipPlaces>(shipPlaces);
        ShipHistory = new List<HistoryService>();
        ContainerHistory = new List<HistoryService>();
    }
    /// <summary>
    /// kj�rer simuleringen til havnen
    /// <summary>
    public void Run()
    {
    }
}