using Measurements;

var baseDir = AppContext.BaseDirectory;
var context = new ApplicationDataContext();
context.LoadData(
    Path.Combine(baseDir, "SensorType.csv"),
    Path.Combine(baseDir, "sensor.csv"),
    Path.Combine(baseDir, "MeasurementType.csv"),
    Path.Combine(baseDir, "Measurement.csv"));

Console.WriteLine("List of Sensors");
Console.WriteLine($"{"Id",2} {"Name",-17}{"Count",6}");
Console.WriteLine(new string('=', 26));
foreach (var s in context.GetSensorsWithMeasurementCount())
{
    Console.WriteLine($"{s.Id,2} {s.Name,-17}{s.Count,6}");
}

Console.WriteLine();
Console.WriteLine("List of Measurement-Types");
Console.WriteLine($"{"Id",2} {"Name",-17}{"Count",6}");
Console.WriteLine(new string('=', 26));
foreach (var mt in context.GetMeasurementTypesWithMeasurementCount())
{
    Console.WriteLine($"{mt.Id,2} {mt.Name,-17}{mt.Count,6}");
}

Console.WriteLine();
Console.WriteLine("List of Sensor by Measurement-Types");
Console.WriteLine($"{"Sensor",-15}{"Measurement",-14}{"Count",10}{"Min",12}{"Max",11}");
Console.WriteLine(new string('=', 62));
foreach (var st in context.GetMeasurementStatisticsBySensorAndType())
{
    Console.WriteLine($"{st.SensorName,-15} {st.MeasurementTypeName,-14}{st.Count,9}{st.Min,12}{st.Max,11}");
}
