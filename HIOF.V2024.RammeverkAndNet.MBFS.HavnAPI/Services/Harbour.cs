using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Services;
public class Harbour : IHarbour
{
    private List<HistoryService> ShipHistory;
    private List<HistoryService> ContainerHistory;
    private List<ShipPlaces> ShipPlacesList;
    private List<Ship> ShipsList;
    private int Timer;

    public Harbour(List<Ship> ships, List<ShipPlaces> shipPlaces)
    {
        ShipsList = new List<Ship>(ships);
        ShipPlacesList = new List<ShipPlaces>(shipPlaces);
        ShipHistory = new List<HistoryService>();
        ContainerHistory = new List<HistoryService>();
    }


    /// <summary>
    /// fjerner skipet fra listen
    /// </summary>
    /// <param name="ship"></param>
    public void RemoveShip(Ship ship)
    {
        ShipsList.Remove(ship);
    }


    /// <summary>
    /// kj�rer simuleringen til havnen
    /// <summary>
    public void Run(DateTime Start, DateTime end)
    {
        //Vi starter med å lage en timer
        DateTime currentTime = Start;

        //Så starter simulasjonen ved bruk av while, der den vil kjøre til sluttdato-en
        while (currentTime < end)
        {
            //Begge for-loops under går gjennom alle ship og plassene
            foreach (var ShipPlace in ShipPlacesList)
            {
                foreach (var ship in ShipsList)
                {
                    //Før det så lager denne metoden Containers objekters til shipet basert på antall i konstruktøren
                    ship.MakeContainers();

                    //Her så vil de se om destinasjonen til skipet og plassen som den itererer
                    if (ship.PlaceDestination.Id == ShipPlace.Id)
                    {

                        //Det skjekker om det er ledig plass i plasssen fra for loop-en
                        if (ShipPlace.AvailableSpace)
                        {
                            if (ship.Repeat == true)
                            {
                                MoveShipFromAnchorage(ShipPlace, ship, currentTime);
                            }
                            else
                            {
                                MoveShipFromAnchorage(ShipPlace, ship, currentTime);
                                ShipsList.Remove(ship);
                            }
                        }

                        //Og hvis det ikke er noen ledig plasser for skipets destinasjon, så flytter vi skipet til en ankerplass
                        else
                        {
                            //Siden vi har to forskjellige lister i en ankerplass (en kø for losseplass og en vanlig liste for kaiplass)
                            //så må vi ha en if test som sjekker først hva slags klasse type plassen er.
                            //Og deretter plassere de i listen/kø-en
                            AddShipToAnchorage(ship, currentTime);
                        }
                    }
                }

                //Etter en flere iterasjoner, så antar vi at alle skipene har seilet samtidig. Da legger vi 60 minutter for hver gang flere skip
                //har nådd destinasjonen
                currentTime.AddMinutes(60);

                //Her så starter vi losse-prossessen
                //If testen sjekker om losseplassen er full og at det er en losseplass før man starter prossessen
                if (!ShipPlace.AvailableSpace && ShipPlace is Unloadingspace)
                {
                    //Her så legger vi til miutter basert på hvor mange Container objekter det er i en skip, hvor mange
                    //skip det er i losseplassen, og hvor fort losse-prossessen er basert på bruker av API-et
                    currentTime.AddMinutes(((Unloadingspace)ShipPlace).UnloadContainer(currentTime));

                    //Etter at alle skipene i losseplassen er ferdig, så returnerer vi listen tilbake til havn klassen
                    AddAllShips(((Unloadingspace)ShipPlace).ReturnShips());

                    //Vi antar at når skipene har blir returnert til havn klassen, så seiler de til Start-of-sea passage som tar 60 min
                    currentTime.AddMinutes(60);

                }
            }
        }
    }
    private Ship MoveShip(Ship TheShip)
    {
        Ship Ship = TheShip;
        ShipsList.Remove(TheShip);
        return Ship;
    }

    private Anchorage GetNextAnchorage() {
        foreach (ShipPlaces Shipplace in ShipPlacesList)
        {
            if(Shipplace is Anchorage && !Shipplace.AvailableSpace)
            {
                return (Anchorage)Shipplace;
            }
        }

        return null;
    }

    private void AddToSpesificPlace(int ShipPlaceId, Ship ship)
    {
        foreach (ShipPlaces Shipplace in ShipPlacesList)
        {
            if(ShipPlaceId == Shipplace.Id)
            {
                Shipplace.AddShip(ship);
            }
        }
    }

    public void AddShip(Ship ship)
    {
        ShipsList.Add(ship);
    }

    public void AddAllShips(List<Ship> Allships)
    {
        ShipsList.AddRange(Allships);
    }

    public void AddShipToAnchorage(Ship ship, DateTime current)
    {
        DateTime CurrentDateTime = current;
        if (ship.PlaceDestination is Unloadingspace)
        {
            CurrentDateTime.AddMinutes(30);
            ship.AddHistory(new HistoryService(ship.PlaceDestination.Name, CurrentDateTime));
            GetNextAnchorage().AddShipToQueue(ship);
        }
        else if (ship.PlaceDestination is Dockspace)
        {
            CurrentDateTime.AddMinutes(30);
            ship.AddHistory(new HistoryService(ship.PlaceDestination.Name, CurrentDateTime));
            GetNextAnchorage().AddShip(ship);
        }
    }

    public void MoveShipFromAnchorage(ShipPlaces shipPlaces, Ship ship, DateTime current)
    {
        DateTime currentDateTime = current;
        if (GetNextAnchorage().ShipQueue.Peek().PlaceDestination.Id == shipPlaces.Id)
        {
            //Her så fjerne vi skipet fra ankerplassen ved bruk av MoveShipFromQueue metoden og
            //plasserer det til destinasjonen ved bruk AddSpesificPlace metoden
            currentDateTime.AddMinutes(30);
            ship.AddHistory(new HistoryService(shipPlaces.Name, currentDateTime));
            AddToSpesificPlace(shipPlaces.Id, ((Anchorage)shipPlaces).MoveShipFromQueue());
        }

        //Og hvis skipet ikke skal til losseplass, så skal den til en kaiplass
        else if (GetNextAnchorage().Ships.First().PlaceDestination.Id == shipPlaces.Id)
        {
            //Her så fjerne vi skipet fra ankerplassen ved bruk av MoveShip metoden og
            //plasserer det til destinasjonen ved bruk AddSpesificPlace metoden
            currentDateTime.AddMinutes(30);
            ship.AddHistory(new HistoryService(shipPlaces.Name, currentDateTime));
            AddToSpesificPlace(shipPlaces.Id, ((Anchorage)shipPlaces).MoveShip(GetNextAnchorage().Ships.First().Id));
        }
        //Hvis ingen av if testene er akseptert, så frakter vi skipet direkte til destinasjonen
        else
        {
            currentDateTime.AddMinutes(60);
            ship.AddHistory(new HistoryService(shipPlaces.Name, currentDateTime));
            shipPlaces.AddShip(MoveShip(ship));
        }
    }
}