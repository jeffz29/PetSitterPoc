# MySQL test bed

`MySqlTestBed` is a reusable fixture for integration tests. It starts MySQL with Docker Compose, optionally applies a supplied SQL schema script, and provides an open `MySqlConnection` for CRUD operations.

```csharp
await using var testBed = new MySqlTestBed();
await testBed.StartAsync("schema.sql");

await using var connection = await testBed.CreateOpenConnectionAsync();
await connection.ExecuteAsync(
    "INSERT INTO Pets (Name) VALUES (@Name);",
    new { Name = "Milo" });
```

The default Docker connection uses `127.0.0.1:3307`, database `petsitter`, and root password `my-secret-pw`. Customize it with `MySqlTestBedOptions`:

```csharp
var options = new MySqlTestBedOptions
{
    ComposeFilePath = Path.Combine("TestData", "docker-compose.yml"),
    ConnectionString = "Server=127.0.0.1;Port=3307;Database=petsitter;User ID=root;Password=my-secret-pw;"
};

await using var testBed = new MySqlTestBed(options);
await testBed.StartAsync("schema.sql");
```