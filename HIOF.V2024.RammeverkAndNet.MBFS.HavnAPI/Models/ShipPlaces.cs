using System;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Exceptions;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models
{
    public abstract class ShipPlaces
    {
        private static int Next = 0;
        public int Id { get; }
        public string Name { get; private set; }
        public int Spaces { get; set; }
        internal List<Ship> Ships { get; }

        /// <summary>
        /// Konstruktøren for ShipPlaces
        /// </summary>
        /// <param name="ShipName">Navnet på plassen, må ikke være tom.</param>
        /// <param name="ShipSpaces">Antallet tilgjengelige plasser. Må være større enn 0.</param>
        /// <exception cref="InvalidNameException">Kastes hvis ShipName er tom.</exception>
        /// <exception cref="InvalidSpacesException">Kastes hvis ShipSpaces er mindre enn eller lik 0.</exception>
        public ShipPlaces(string ShipName, int ShipSpaces)
        {
            if (string.IsNullOrWhiteSpace(ShipName))
            {
                throw new InvalidNameException("ShipName cannot be empty");
            }

            if (ShipSpaces <= 0)
            {
                throw new InvalidSpacesException("ShipSpaces must be greater than 0");
            }

            Name = ShipName;
            Spaces = ShipSpaces;
            Ships = new List<Ship>();
            Id = Interlocked.Increment(ref Next);
        }

        /// <summary>
        /// Metoden legger til ship i de plassene for å simulere at de ha nådd denne plassen
        /// </summary>
        /// <param name="ship"></param>
        internal virtual void AddShip(Ship ship)
        {
            Ships.Add(ship);
        }

        /// <summary>
        /// En metode som flytter ship fra og til en annen plass under simuleringen.
        /// </summary>
        /// <param name="id"></param>
		/// <returns></returns>
        internal virtual Ship MoveShip(int id)
        {
            Ship TheShip;
            foreach (var SpesificShip in Ships)
            {
                if (SpesificShip.Id == id)
                {
                    TheShip = SpesificShip;
                    Ships.Remove(SpesificShip);
                    return TheShip;
                }
            }
            return null;
        }

        /// <summary>
        /// Det fjerner alle shipene som har repeterende seilinger fra lista. Det blir brukt i simulasjonen.
        /// </summary>
        /// <returns></returns>
        internal List<Ship> ReturnRepeatingShips()
        {
            List<Ship> OldShips = new List<Ship>();
            foreach (Ship ship in Ships)
            {
                if (ship.Repeat is true)
                {
                    OldShips.Add(ship);
                    Ships.Remove(ship);
                }
            }
            return OldShips;
        }


        /// <summary>
        /// Det fjerner alle shipene fra lista. Det blir brukt i simulasjonen.
        /// </summary>
        /// <returns></returns>
        internal virtual List<Ship> ReturnAllShips()
        {
            List<Ship> OldShips = new List<Ship>(Ships);
            Ships.Clear();
            return OldShips;
        }

        /// <summary>
        /// Denen metoden er for å se om det er ledig plasser
        /// </summary>
        /// <returns></returns>
        internal virtual bool AvailableSpace
        {
            get
            {
                return Spaces > Ships.Count;
            }
        }




    }
}

