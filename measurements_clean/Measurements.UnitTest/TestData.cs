namespace Measurements.UnitTest;

// shortened versions of the given CSV files
internal static class TestData
{
    public static readonly string[] SensorTypeLines =
    [
        "Id;Name",
        "1;Gosund EP2",
        "2;Tasmota SW",
        "3;Tasmota Sensor"
    ];

    public static readonly string[] SensorLines =
    [
        "Id;Name;FirstSeen;SensorTypeId",
        "1;tasmota_PC;2022/11/30 21:25:44;1",
        "4;tasmota_Server;2022/12/01 15:52:59;1",
        "6;tasmota_SW1;2022/12/03;2",
        "9;tasmota_Temp;2022/12/04 19:52:12;3"
    ];

    public static readonly string[] MeasurementTypeLines =
    [
        "Id;Name;Comment",
        "1;Total;",
        "4;Power;",
        "10;Temperature;",
        "14;eCO2;"
    ];

    public static readonly string[] MeasurementLines =
    [
        "Id;Value;Time;SensorId;MeasurementTypeId",
        "1;0.529;2022/11/30 21:25:44;1;1",
        "2;63;2022/11/30 21:25:44;1;4",
        "3;0.531;2022/11/30 21:30:44;1;1",
        "4;120;2022/11/30 21:30:44;1;4",
        "5;0;2022/11/30 21:35:44;1;4",
        "6;48;2022/12/01 15:52:59;4;4",
        "7;119;2022/12/01 15:57:59;4;4",
        "8;21.5;2022/12/04 19:52:12;9;10",
        "9;22.3;2022/12/04 19:57:12;9;10",
        "10;598;2022/12/04 19:57:12;9;14"
    ];

    public static ApplicationDataContext CreateContext()
    {
        var context = new ApplicationDataContext();
        context.LoadData(SensorTypeLines, SensorLines, MeasurementTypeLines, MeasurementLines);
        return context;
    }
}
