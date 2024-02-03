using System;
using System.Collections.ObjectModel;
namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models
{
	public class Container
	{
		public int ID { get; private set; }
		public List<History> Histories { get; private set; }

		public Container(int id)
		{
            ID = id;
            Histories = new List<HistoryService>();
        }
	}
}

