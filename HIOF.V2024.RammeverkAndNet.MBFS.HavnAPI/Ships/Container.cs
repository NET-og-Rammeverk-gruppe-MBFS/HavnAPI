using System.Collections.ObjectModel;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
public class Container
{
    /// <summary>
    /// ID til en container. ID er autogenerert
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Liste over alle loggførte plassering til container
    /// </summary>
    public ReadOnlyCollection<HistoryService> Histories
    {
        get { return new ReadOnlyCollection<HistoryService>(HistoriesInternal); }
    }

    /// <summary>
    /// Dette forteller hva slags type container det er
    /// </summary>
    public ContainerType Type { get; private set; }

    internal Collection<HistoryService> HistoriesInternal { get; set; }
    private static int Next = 0;
    internal Container(ContainerType containerType)
    {
        Id = Interlocked.Increment(ref Next);
        HistoriesInternal = new Collection<HistoryService>();
        Type = containerType;
    }
}

