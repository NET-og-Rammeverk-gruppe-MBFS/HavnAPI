using System.Collections.ObjectModel;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Locations
{
    public class StorageColumn
    {
        /// <summary>
        /// For å lage en lagringskolonne
        /// </summary>
        /// <param name="numberOfCranesInColumn">Antall kraner i lagringskolonnen</param>
        /// <param name="numberOfColumns"> Antall kolonner i lagringskolonnen</param>
        /// <param name="width">Bredden basert på antall containere i en kolonne</param>
        /// <param name="height">høyde basert på antall containere i en kolonne</param>
        public StorageColumn(int numberOfCranesInColumn,int numberOfColumns, int width, int height)
        {
            NumberOfCranes = numberOfCranesInColumn;
            Columns = new Collection<Column>();
            for (int i = 0; i < numberOfColumns; i++) 
            {
                Columns.Add(new Column(width, height));
            }
        }

        internal Collection<Column> Columns { get; private set; }
        internal int NumberOfCranes { get; private set; }
    }
}
