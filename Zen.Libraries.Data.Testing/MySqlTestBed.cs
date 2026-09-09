using System.Diagnostics;
using MySqlConnector;

namespace Zen.Libraries.Data.Testing;

public sealed class MySqlTestBed : IAsyncDisposable
{
    private readonly MySqlTestBedOptions options;
    private bool started;

    public MySqlTestBed(MySqlTestBedOptions? options = null)
    {
        this.options = options ?? new MySqlTestBedOptions();
    }

    public string ConnectionString => options.ConnectionString;

    public async Task StartAsync(
        string? schemaScriptPath = null,
        CancellationToken cancellationToken = default)
    {
        if (started)
        {
            throw new InvalidOperationException("The MySQL test bed has already been started.");
        }

        await RunDockerComposeAsync("up", "-d", "--wait", cancellationToken);
        started = true;

        if (schemaScriptPath is not null)
        {
            await using var connection = await CreateOpenConnectionAsync(cancellationToken);
            await new MySqlScriptRunner().RunAsync(connection, schemaScriptPath, cancellationToken);
        }
    }

    public async Task<MySqlConnection> CreateOpenConnectionAsync(
        CancellationToken cancellationToken = default)
    {
        if (!started)
        {
            throw new InvalidOperationException("Start the MySQL test bed before creating a connection.");
        }

        var connection = new MySqlConnection(ConnectionString);
        try
        {
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
        catch
        {
            await connection.DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!started)
        {
            return;
        }

        try
        {
            await RunDockerComposeAsync("down", cancellationToken: CancellationToken.None);
        }
        finally
        {
            started = false;
        }
    }

    private async Task RunDockerComposeAsync(
        string command,
        string? firstArgument = null,
        string? secondArgument = null,
        CancellationToken cancellationToken = default)
    {
        var arguments = new List<string>
        {
            "compose",
            "-f",
            options.ComposeFilePath,
            "-p",
            options.ComposeProjectName,
            command
        };

        if (firstArgument is not null)
        {
            arguments.Add(firstArgument);
        }

        if (secondArgument is not null)
        {
            arguments.Add(secondArgument);
        }

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "docker",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        foreach (var argument in arguments)
        {
            processStartInfo.ArgumentList.Add(argument);
        }

        using var process = new Process { StartInfo = processStartInfo };
        if (!process.Start())
        {
            throw new InvalidOperationException("Unable to start the Docker process.");
        }

        var standardOutput = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var standardError = process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);
        var error = await standardError;
        await standardOutput;

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Docker Compose '{command}' failed with exit code {process.ExitCode}: {error}");
        }
    }
}