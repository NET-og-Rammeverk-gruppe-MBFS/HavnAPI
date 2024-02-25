using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Services;
namespace HIOF.V2024.RammeverkAndNet.MBFS.TestClient;
class Oppgave_2
{
    static void Main(string[] args)
    {
        // Lag 3 kaiplasser (dockspace), 2 ankerplasser (anchorage), og 2 losseplasser (unloadingspace).
        Dockspace kaiplass1 = new Dockspace("Liten_kaiplass", 2);
        Dockspace kaiplass2 = new Dockspace("Middels_kaiplass", 4);
        Dockspace kaiplass3 = new Dockspace("Stor_kaiplass", 6);

        Anchorage venteplass1 = new Anchorage("Liten_venteplass", 1);
        Anchorage venteplass2 = new Anchorage("Middels_venteplass", 3);

        Unloadingspace losseplass1 = new Unloadingspace("Liten_losseplass", 1, 10, 5);
        Unloadingspace losseplass2 = new Unloadingspace("Stor_losseplass", 5, 80, 10);

        List<ShipPlaces> shipPlaces = new List<ShipPlaces>();
        shipPlaces.Add(kaiplass1);
        shipPlaces.Add(kaiplass2);
        shipPlaces.Add(kaiplass3);
        shipPlaces.Add(venteplass1);
        shipPlaces.Add(venteplass2);
        shipPlaces.Add(losseplass1);
        shipPlaces.Add(losseplass2);

        // Lag bestemt antall skip objekter og knytt de til kaiplasser og losseplasser du lagde tidligere.

    }
}