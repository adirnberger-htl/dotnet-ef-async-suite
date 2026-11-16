namespace Measurements.UnitTest;

public class QueryTests
{
    [Fact]
    public void GetSensorsWithMeasurementCount_ReturnsAllSensorsOrderedById()
    {
        var context = TestData.CreateContext();

        var result = context.GetSensorsWithMeasurementCount();

        Assert.Equal(
        [
            new SensorMeasurementCount(1, "tasmota_PC", 5),
            new SensorMeasurementCount(4, "tasmota_Server", 2),
            new SensorMeasurementCount(6, "tasmota_SW1", 0),
            new SensorMeasurementCount(9, "tasmota_Temp", 3)
        ], result);
    }

    [Fact]
    public void GetMeasurementTypesWithMeasurementCount_ReturnsAllTypesOrderedById()
    {
        var context = TestData.CreateContext();

        var result = context.GetMeasurementTypesWithMeasurementCount();

        Assert.Equal(
        [
            new MeasurementTypeMeasurementCount(1, "Total", 2),
            new MeasurementTypeMeasurementCount(4, "Power", 5),
            new MeasurementTypeMeasurementCount(10, "Temperature", 2),
            new MeasurementTypeMeasurementCount(14, "eCO2", 1)
        ], result);
    }

    [Fact]
    public void GetMeasurementStatisticsBySensorAndType_GroupsAndSortsBySensorAndType()
    {
        var context = TestData.CreateContext();

        var result = context.GetMeasurementStatisticsBySensorAndType();

        Assert.Equal(
        [
            new SensorMeasurementTypeStatistic("tasmota_PC", "Power", 3, 0, 120),
            new SensorMeasurementTypeStatistic("tasmota_PC", "Total", 2, 0.529, 0.531),
            new SensorMeasurementTypeStatistic("tasmota_Server", "Power", 2, 48, 119),
            new SensorMeasurementTypeStatistic("tasmota_Temp", "eCO2", 1, 598, 598),
            new SensorMeasurementTypeStatistic("tasmota_Temp", "Temperature", 2, 21.5, 22.3)
        ], result);
    }
}
