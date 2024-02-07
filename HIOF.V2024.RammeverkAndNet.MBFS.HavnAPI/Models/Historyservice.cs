/// <summary>
/// representerer en historikken til skips plass
/// </summary>
public class HistoryService
{

    /// <summary>
    /// F�r navnet p� stedet hvor skipet ligger.
    /// </summary>
    public string Name { get; private set; }
    
    public DateTime Time { get; private set; }

    /// <summary>
    /// Oppretter en ny instans av <see cref="HistoryService"/>-klassen for � holde styr p� historiske hendelser.
    /// Dette lar deg lagre et stedsnavn sammen med et tidspunkt for en hendelse, noe som er nyttig for � organisere og hente historisk informasjon.
    /// </summary>
    /// <param name="placeName">Navnet p� stedet hvor hendelsen skjedde.</param>
    /// <param name="entryTime">Tidspunktet for hendelsen.</param>
    public HistoryService(string placeName, DateTime entryTime)
    {
        Name = placeName;
        Time = entryTime;
    }

    public override string ToString()
    {
        return $"{Name} - {Time}";
    }
}