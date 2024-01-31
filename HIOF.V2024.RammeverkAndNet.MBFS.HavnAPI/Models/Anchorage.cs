using System;
using System.Runtime.CompilerServices;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Abstract;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models

public class Anchorage : ShipPlaces
{
    public Anchorage(string Name, int Spaces) : base(name, spaces)
    {
    }
}