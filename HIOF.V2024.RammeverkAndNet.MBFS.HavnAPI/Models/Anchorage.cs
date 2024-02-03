using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models


public class Anchorage : ShipPlaces
{
    public Anchorage(string Name, int Spaces) : base(name, spaces)
    {
    }

    /// <summary>
    /// Flytter skipet fra plass til en annen plass
    /// <summary>
    /// <param name="id">Id til skipet som flyttes</param>
    public override Ship MoveShip(int id)
    {
    }

    /// <summary>
    /// Legger til et  skip til ankerplassen
    /// <summary>
    /// <param name="ship">Skipet som skal legges til</param>
    public override void AddShip(Ship ship)
    {
    }
}