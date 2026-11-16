using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

//TODO 4 If model and unit tests work, comment in this demo program
/*

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
			await InitAsync();
			Console.WriteLine("Initialized.");
			break;
		case "clear":
			await ClearAsync();
			Console.WriteLine("Cleared.");
			break;
		case "classes":
			await ShowClassesAsync();
			break;
		case "pupils":
			var className = parts.Length > 1 ? string.Join(' ', parts.Skip(1)) : null;
			await ShowPupilsAsync(className);
			break;
		case "exit":
			return;
		default:
			Console.WriteLine("Unknown command.");
			break;
	}
}

static async Task InitAsync()
{
	await ClearAsync();

	await using var db = new ApplicationDbContext();

	var classA = new SchoolClass { Name = "1A" };
	var classB = new SchoolClass { Name = "1B" };
	
	var maria = new Teacher { FirstName = "Maria", LastName = "Mayer" };
	var thomas = new Teacher { FirstName = "Thomas", LastName = "Huber" };

	classA.Pupils.Add(new Pupil
	{
		FirstName = "Anna",
		LastName = "Berg",
		BirthDate = UtcDate(2010, 4, 12),
		SchoolLaptop = new SchoolLaptop
		{
			SerialNr = "LT-1001",
			MakeAndModel = "Lenovo ThinkPad",
			InstalledSWImage = "School-2026",
			Administrator = maria
		}
	});
	classA.Pupils.Add(new Pupil
	{
		FirstName = "Liam",
		LastName = "Frey",
		BirthDate = UtcDate(2011, 9, 3)
	});
	classA.Pupils.Add(new Pupil { FirstName = "Noah", LastName = "Keller", BirthDate = UtcDate(2012, 3, 5) });
	classA.Pupils.Add(new Pupil { FirstName = "Mia", LastName = "Schmid", BirthDate = UtcDate(2012, 1, 19) });
	classA.Pupils.Add(new Pupil { FirstName = "Eva", LastName = "Weiss", BirthDate = UtcDate(2011, 6, 27) });

	classB.Pupils.Add(new Pupil
	{
		FirstName = "Luca",
		LastName = "Ammann",
		BirthDate = UtcDate(2010, 11, 8),
		SchoolLaptop = new SchoolLaptop
		{
			SerialNr = "LT-1002",
			MakeAndModel = "Dell Latitude",
			InstalledSWImage = "School-2026",
			Administrator = thomas
		}
	});
	classB.Pupils.Add(new Pupil { FirstName = "Lea", LastName = "Brunner", BirthDate = UtcDate(2011, 2, 14) });
	classB.Pupils.Add(new Pupil { FirstName = "Nina", LastName = "Graf", BirthDate = UtcDate(2012, 5, 20) });
	classB.Pupils.Add(new Pupil { FirstName = "Ben", LastName = "Huber", BirthDate = UtcDate(2011, 8, 9) });
	classB.Pupils.Add(new Pupil { FirstName = "Jan", LastName = "Moser", BirthDate = UtcDate(2012, 10, 2) });

	var mathematics = new Subject { Title = "Mathematics" };
	mathematics.Teachers.Add(maria);
	mathematics.SchoolClasses.Add(classA);
	mathematics.SchoolClasses.Add(classB);

	var english = new Subject { Title = "English" };
	english.Teachers.Add(thomas);
	english.SchoolClasses.Add(classA);

	await db.SchoolClasses.AddRangeAsync(classA, classB);
	await db.Subjects.AddRangeAsync(mathematics, english);
	await db.SaveChangesAsync();
}

static async Task ClearAsync()
{
	await using var db = new ApplicationDbContext();
	await using var transaction = await db.Database.BeginTransactionAsync();

	await db.SchoolLaptops.ExecuteDeleteAsync();
	await db.Pupils.ExecuteDeleteAsync();
	await db.SchoolClasses.ExecuteDeleteAsync();
	await db.Subjects.ExecuteDeleteAsync();
	await db.Teachers.ExecuteDeleteAsync();

	await transaction.CommitAsync();
}

static async Task ShowClassesAsync()
{
	await using var db = new ApplicationDbContext();

	var classes = await db.SchoolClasses
		.OrderBy(c => c.Name)
		.Select(c => c.Name)
		.ToListAsync();

	if (classes.Count == 0)
	{
		Console.WriteLine("No classes.");
		return;
	}

	foreach (var className in classes)
	{
		Console.WriteLine(className);
	}
}

static async Task ShowPupilsAsync(string? className)
{
	await using var db = new ApplicationDbContext();

	IQueryable<Pupil> query = db.Pupils.Include(p => p.SchoolClass);

	if (!string.IsNullOrWhiteSpace(className))
	{
		query = query.Where(p => p.SchoolClass.Name == className);
	}

	var pupils = await query
		.OrderBy(p => p.LastName)
		.ThenBy(p => p.FirstName)
		.ToListAsync();

	if (pupils.Count == 0)
	{
		Console.WriteLine("No pupils.");
		return;
	}

	foreach (var p in pupils)
	{
		Console.WriteLine($"{p.LastName}, {p.FirstName} ({p.SchoolClass.Name})");
	}
}

static DateTime UtcDate(int year, int month, int day) =>
	new(year, month, day, 0, 0, 0, DateTimeKind.Utc);

*/