using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    internal class ContainerSpace
    {
        internal Collection<StorageColumn> StorageColumns { get; private set; }
        public int NumberOfAGVs { get; private set; }

        public ContainerSpace(int numberOfAGVs)
        {
            NumberOfAGVs = numberOfAGVs;
            StorageColumns = new Collection<StorageColumn>();
        }

        public void AddStorageColumn(StorageColumn storageColumn)
        {
            StorageColumns.Add(storageColumn);
        }
    }
}
