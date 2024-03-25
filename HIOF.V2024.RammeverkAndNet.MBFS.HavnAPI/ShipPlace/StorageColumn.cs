using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    public class StorageColumn
    {
        internal Collection<Column> Columns { get; private set; }
        public int NumberOfCranes { get; private set; }

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
