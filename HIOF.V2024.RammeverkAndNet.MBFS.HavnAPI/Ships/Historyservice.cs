namespace HIOF.V2024.RammeverkAndNet.MBFS.HavnAPI.Ships;
/// <summary>
/// representerer en historikken til skips plass
/// </summary>
public class HistoryService
{

    /// <summary>
    /// Navnet til objekten (Container eller Ship).
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Dato når historikken ble lagd
    /// </summary>
    public DateTime Time { get; private set; }
    /// <summary>
    /// Forklaring for denne historikken. Denne blir mest brukt for å fortelle når et skip eller container er sist plassert
    /// </summary>
    public string Description { get; private set; }

    public override string ToString()
    {
        return $"{Name} - {Time} - {Description}";
    }
    
    internal HistoryService(string historyName, DateTime entryTime, string historyDescription)
    {
        Description = historyDescription;
        Name = historyName;
        Time = entryTime;
    }
}