using System.Collections.ObjectModel;

namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;

public class Container
{
    private static int Next = 0;
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

    internal Collection<HistoryService> HistoriesInternal { get; set; }
    /// <summary>
    /// Dette forteller hva slags type container det er
    /// </summary>
    public ContainerType Type { get; private set; }

    /// <summary>
    /// For å lage Container
    /// </summary>
    /// <param name="containerType"> Sette en container type som bruker enum ContainerType</param>
    internal Container(ContainerType containerType)
    {
        Id = Interlocked.Increment(ref Next);
        HistoriesInternal = new Collection<HistoryService>();
        Type = containerType;
    }
}

