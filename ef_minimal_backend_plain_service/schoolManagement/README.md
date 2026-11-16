# School Management - Pupils and Classes

This is the minimum setup of an Entity Framework project.

## Structure

- SchoolManagementConsole: Program interacting with school data
- Services
  - SchoolMgmtService: School-management operations and queries
- Persistence: Provides data model and storage
  - Entities: Model
  - ApplicationDbContext: Gateway to storage
  - Underlying relational DB: Microsoft SQL Server / LocalDB
- PersistenceIntegrationTest
  - xunit based tests against Persistence
  - uses full DB instance, with maybe own DB
  - at least a non-committed transaction per testcase

## Prerequisites

- Install dotnet ef: `dotnet tool install --globale dotnet-ef`
- Create a MSSql docker/podman:
```bash
docker run \
  --name mssql \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=one2Three$" \
  -p 1433:1433 \
  -v mssql-data:/var/opt/mssql \
  -d \
  mcr.microsoft.com/mssql/server:2025-latest
  ```
  - Connect to this server with a DB Client (VS Code or beaver). U:sa P:one2Three$

## Create project outline

- console SchoolManagementConsole
- classlib Persistence
- xunit PersistenceIntegrationTest

## Persistence

### Entity Framework Scaffold

- Add the following project references:
```shell
cd Persistence

dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.Configuration.Json
```
- Create `ApplicationDbContext.cs`. The context loads the connection string from the
  `appsettings.json` file copied beside the running application:
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration["ConnectionStrings:Default"]
                ?? throw new InvalidOperationException("The default connection string is missing.");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Add more model constraints for the DB  here
        }
    }
}

```
- Runtime applications, tests, and EF tools can construct the context directly:
```cs
using var db = new ApplicationDbContext();
```
- Create `appsettings.json`
```json
{
  "ConnectionStrings": { 
    "Default": "Data Source=localhost;Initial Catalog=SchoolManagement;User Id=sa;Password=one2Three$;TrustServerCertificate=True;ApplicationIntent=ReadWrite", }
}
```
- Edit Persistence.csproj to always copy the appsettings-file:
```xml
  <ItemGroup>
    <None Update="appsettings.json">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  <ItemGroup>
```
- Check if dotnet ef is happy:
```shell
dotnet ef database drop
```
### Setup the Data Model

- Create a namespace / directory Persistence / Entities.
- Create a model of a SchoolClass as shown in model.puml; ignore the constraints for now.
- Check if it builds correctly.
- Add the `DbSet` for the model to `ApplicationDbContext`.
- Create the initial migration: 
  - `dotnet ef migrations add initial`
  - `dotnet ef database update`
- Inspect the DB with a client. It must show three tables:
  - Pupil, SchoolClass: From the model
  - \_EFMigrationsHistory: Housekeeping of entity framework. NEVER tinker with this (unless you're Senior Dev and three other Seniors agree to look the other way)

## Integration Tests

- Reference `Persistence` from PersistenceIntegrationTests.
- Create a test class `ApplicationDbContextTests`
- Create a test setup method that creates an `ApplicationDbContext` and opens a transaction
- Implement a test case Fact that checks that the DBContext is there and has the expected DBSets (if empty)

## Console App

- Reference `Persistence` and `Services` from SchoolManagementConsole
- Implement a minimal menu:
  - init - initialize with 2 classes of 5 pupils each
  - clear - remove all data
  - classes - show classes
  - pupils class - show pupils in given class. if no class is given, show pupils, ordered by last name.
  - exit - quit program
- Directly use the dbcontext in a variable using.
- Keep it very small.
