namespace ExportAnalyzer.Entities;

public class ExportData
{
    private static int _idCounter = 0;
    public int Id { get; private set; }

    public required Commodity Commodity { get; init; }

    public required Country Country { get; init; }

    public required double TradeValue { get; init; }

    public ExportData()
    {
        Id = _idCounter++;
    }
}
