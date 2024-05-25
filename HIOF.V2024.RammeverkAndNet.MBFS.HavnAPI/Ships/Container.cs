using System.Collections.ObjectModel;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Container
{
	private static int next = 0;
	/// <summary>
	/// ID til en container. ID er autogenerert
	/// </summary>
	public int id { get; private set; }
	internal Collection<HistoryService> histories { get; private set; }
	/// <summary>
	/// Dette forteller hva slags type container det er
	/// </summary>
	public ContainerType type { get; private set; }

	/// <summary>
	/// For å lage Container
	/// </summary>
	/// <param name="containerType"> Sette en container type som bruker enum ContainerType</param>
	internal Container(ContainerType containerType)
	{
		id = Interlocked.Increment(ref next);
		histories = new Collection<HistoryService>();
		type = containerType;
		}
}

