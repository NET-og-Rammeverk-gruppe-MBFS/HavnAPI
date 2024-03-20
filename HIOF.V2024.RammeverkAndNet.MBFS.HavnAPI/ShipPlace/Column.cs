using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.ShipPlace
{
    internal class Column
    {
        internal Collection<Stack<Container>> column;
        public int amount {get; private set; }
        public int max {get; private set; }
        internal ContainerType ContainerType;

        internal Column()
        {
        }
    }
}
