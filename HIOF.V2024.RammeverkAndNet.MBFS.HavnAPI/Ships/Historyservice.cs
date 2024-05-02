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

    /// <summary>
    /// Oppretter en ny instans av <see cref="HistoryService"/>-klassen for � holde styr p� historiske hendelser.
    /// Dette lar deg lagre et stedsnavn sammen med et tidspunkt for en hendelse, noe som er nyttig for � organisere og hente historisk informasjon.
    /// </summary>
    /// <param name="name">Navnet p� stedet hvor hendelsen skjedde.</param>
    /// <param name="entryTime">Tidspunktet for hendelsen.</param>
    internal HistoryService(string name, DateTime entryTime, string description)
    {
        Description = description;
        Name = name;
        Time = entryTime;
    }

    public override string ToString()
    {
        return $"{Name} - {Time} - {Description}";
    }
}