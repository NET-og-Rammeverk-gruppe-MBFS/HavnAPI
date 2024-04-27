using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    public class StorageColumn
    {
        internal Collection<Column> Columns { get; private set; }
        internal int NumberOfCranes { get; private set; }

        /// <summary>
        /// For å lage en lagringskolonne
        /// </summary>
        /// <param name="numberOfCranes">Antall kraner i lagringskolonnen</param>
        /// <param name="numberOfColumns"> Antall kolonner i lagringskolonnen</param>
        /// <param name="width">Bredden basert på antall containere i en kolonne</param>
        /// <param name="height">høyde basert på antall containere i en kolonne</param>
        public StorageColumn(int numberOfCranes,int numberOfColumns, int width, int height)
        {
            NumberOfCranes = numberOfCranes;
            Columns = new Collection<Column>();
            for (int i = 0; i < numberOfColumns; i++) 
            {
                Columns.Add(new Column(width, height));
            }

        }
    }
}
