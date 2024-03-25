using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    public class ContainerSpace
    {
        public string Name { get; private set; }
        internal Collection<StorageColumn> StorageColumns { get; private set; }
        public Collection<AGV> AGVs { get; private set; }

        public ContainerSpace(int numberOfAGVs)
        {
            AGVs = new Collection<AGV>();
            StorageColumns = new Collection<StorageColumn>();

            for (int i = 0; i < numberOfAGVs; i++)
            {
                AGVs.Add(new AGV());
            }
        }

        public void AddStorageColumn(int amount, int numberOfCranes, int length, int width, int height)
        {
            for (int id = 1; id <= amount; id++)
            {
                StorageColumn newColumn = new StorageColumn(numberOfCranes, id, width, height);
                StorageColumns.Add(newColumn);
            }
        }
    }
}
