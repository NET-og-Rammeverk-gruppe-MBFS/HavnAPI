namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Models
{
	public class Container
	{
		private static int Next = 0;
		public int ID { get; }
		public List<HistoryService> Histories { get; private set; }

		public Container()
		{
			ID = Interlocked.Increment(ref Next);
            Histories = new List<HistoryService>();
        }
	}
}

