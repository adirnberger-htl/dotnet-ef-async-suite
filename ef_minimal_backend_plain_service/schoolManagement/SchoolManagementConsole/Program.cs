using Persistence;
using Services;

await using var context = new ApplicationDbContext();
var service = new SchoolMgmtService(context);

while (true)
{
	Console.Write("Command (init|clear|classes|pupils [class]|exit): ");
	var input = Console.ReadLine()?.Trim() ?? string.Empty;
	var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
	if (parts.Length == 0)
	{
		continue;
	}

	var command = parts[0].ToLowerInvariant();
	switch (command)
	{
		case "init":
			await service.InitAsync();
			Console.WriteLine("Initialized.");
			break;
		case "clear":
			await service.ClearAsync();
			Console.WriteLine("Cleared.");
			break;
		case "classes":
			await ShowClasses(service);
			break;
		case "pupils":
			var className = parts.Length > 1 ? string.Join(' ', parts.Skip(1)) : null;
			await ShowPupils(service, className);
			break;
		case "exit":
			return;
		default:
			Console.WriteLine("Unknown command.");
			break;
	}
}

static async Task ShowClasses(SchoolMgmtService service)
{
	var classes = await service.GetSchoolClassesAsync();

	if (classes.Count == 0)
	{
		Console.WriteLine("No classes.");
		return;
	}

	foreach (var schoolClass in classes)
	{
		Console.WriteLine(schoolClass.Name);
	}
}

static async Task ShowPupils(SchoolMgmtService service, string? className)
{
	var pupils = await service.GetPupilsAsync(className);

	if (pupils.Count == 0)
	{
		Console.WriteLine("No pupils.");
		return;
	}

	foreach (var p in pupils)
	{
		Console.WriteLine($"{p.LastName}, {p.FirstName} ({p.SchoolClass?.Name ?? "-"})");
	}
}
