using System;

public class HistoryService
{
    public string NameToShipPlace { get; private set; }
    public DateTime Time { get; private set; }

    public HistoryService(string placeName, DateTime entryTime)
    {
        NameToShipPlace = placeName;
        Time = entryTime;
    }
}