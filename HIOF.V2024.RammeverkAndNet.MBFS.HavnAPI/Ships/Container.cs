using System.Collections.ObjectModel;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Container
{
	private static int Next = 0;
	public int ID { get; private set; }
	internal Collection<HistoryService> Histories { get; private set; }
	public ContainerType Type { get; private set; }

	/// <summary>
	/// For å lage Container
	/// </summary>
	/// <param name="containerType"> Sette en container type som bruker enum ContainerType</param>
	internal Container(ContainerType containerType)
	{
		ID = Interlocked.Increment(ref Next);
		Histories = new Collection<HistoryService>();
		Type = containerType;
		}
}

