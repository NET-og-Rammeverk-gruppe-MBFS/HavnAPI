using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Enums;
using System;

public class Ship
{
    public int ShipId { get; set; }
    public ShipType Type { get; set; }

    public int Cargospace { get; set; }
    public int Passangerspace { get; set; }
    public float Tankcapacity { get; set; }

    public Ship(int shipId, ShipType type, int cargospace, int passangerspace, float tankcapacity)
    {
        ShipId = shipId;
        Type = type;
        Cargospace = cargospace;
        Passangerspace = passangerspace;
        Tankcapacity = tankcapacity;
    }

    public void simulationInformation()
    {
        switch (Type)
        {
            case ShipType.Cargo:
                Console.Writeline($"Ship {ShipId} is a {Type} with a cargocapacity of {Cargospace}");
                break;
            case ShipType.Passenger:
                Console.Writeline($"Ship {ShipId} is a {Type} with a passangercapacity of {Passangerspace}");
                break;
            case ShipType.Tank:
                Console.Writeline($"Ship {ShipId} is a {Type} with a tankcapacity of {Tankcapacity}");
                break;
        }
    }
}