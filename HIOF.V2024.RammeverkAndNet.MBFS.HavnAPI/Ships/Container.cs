using System.Collections.ObjectModel;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Container
{
	private static int Next = 0;
	public int ID { get; }
	public Collection<HistoryService> Histories { get; private set; }

	public Container()
	{
		ID = Interlocked.Increment(ref Next);
            Histories = new Collection<HistoryService>();
        }
}

