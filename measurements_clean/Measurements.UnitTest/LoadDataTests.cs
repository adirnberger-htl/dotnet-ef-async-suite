namespace Measurements.UnitTest;

public class LoadDataTests
{
    [Fact]
    public void LoadData_CreatesAllEntities()
    {
        var context = TestData.CreateContext();

        Assert.Equal(3, context.SensorTypeDataSet.Count);
        Assert.Equal(4, context.SensorDataSet.Count);
        Assert.Equal(4, context.MeasurementTypeDataSet.Count);
        Assert.Equal(10, context.MeasurementDataSet.Count);
    }

    [Fact]
    public void LoadData_ParsesValuesCultureInvariant()
    {
        var context = TestData.CreateContext();

        var measurement = context.MeasurementDataSet.Single(m => m.Id == 1);
        Assert.Equal(0.529, measurement.Value);
        Assert.Equal(new DateTime(2022, 11, 30, 21, 25, 44), measurement.Time);

        var sensor = context.SensorDataSet.Single(s => s.Id == 6);
        Assert.Equal(new DateTime(2022, 12, 3), sensor.FirstSeen);
    }

    [Fact]
    public void LoadData_SensorReferencesSensorType()
    {
        var context = TestData.CreateContext();

        var sensor = context.SensorDataSet.Single(s => s.Name == "tasmota_Temp");
        Assert.Same(context.SensorTypeDataSet.Single(st => st.Id == 3), sensor.SensorType);
    }

    [Fact]
    public void LoadData_SensorTypeContainsItsSensors()
    {
        var context = TestData.CreateContext();

        var sensorType = context.SensorTypeDataSet.Single(st => st.Id == 1);
        Assert.Equal([1, 4], sensorType.Sensors.Select(s => s.Id).Order());
    }

    [Fact]
    public void LoadData_MeasurementReferencesSensorAndMeasurementType()
    {
        var context = TestData.CreateContext();

        var measurement = context.MeasurementDataSet.Single(m => m.Id == 8);
        Assert.Same(context.SensorDataSet.Single(s => s.Id == 9), measurement.Sensor);
        Assert.Same(context.MeasurementTypeDataSet.Single(mt => mt.Id == 10), measurement.MeasurementType);
    }

    [Fact]
    public void LoadData_SensorAndMeasurementTypeContainTheirMeasurements()
    {
        var context = TestData.CreateContext();

        var sensor = context.SensorDataSet.Single(s => s.Id == 4);
        Assert.Equal([6, 7], sensor.Measurements.Select(m => m.Id).Order());

        var measurementType = context.MeasurementTypeDataSet.Single(mt => mt.Id == 1);
        Assert.Equal([1, 3], measurementType.Measurements.Select(m => m.Id).Order());

        Assert.Empty(context.SensorDataSet.Single(s => s.Id == 6).Measurements);
    }

    [Fact]
    public void LoadData_CalledTwice_DoesNotDuplicateRelations()
    {
        var context = TestData.CreateContext();
        context.LoadData(TestData.SensorTypeLines, TestData.SensorLines, TestData.MeasurementTypeLines, TestData.MeasurementLines);

        Assert.Equal(10, context.MeasurementDataSet.Count);
        Assert.Equal(2, context.SensorDataSet.Single(s => s.Id == 4).Measurements.Count);
    }
}
