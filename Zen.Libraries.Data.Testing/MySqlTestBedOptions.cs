namespace Zen.Libraries.Data.Testing;

public sealed class MySqlTestBedOptions
{
    public string ComposeFilePath { get; init; } =
        Path.Combine(AppContext.BaseDirectory, "docker-compose.yml");

    public string ConnectionString { get; init; } =
        "Server=127.0.0.1;Port=3307;Database=petsitter;User ID=root;Password=my-secret-pw;";

    public string ComposeProjectName { get; init; } = "zen-mysql-testbed";
}