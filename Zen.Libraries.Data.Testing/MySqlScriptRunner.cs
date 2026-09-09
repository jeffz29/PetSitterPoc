using MySqlConnector;

namespace Zen.Libraries.Data.Testing;

public sealed class MySqlScriptRunner
{
    public async Task<int> RunAsync(
        MySqlConnection connection,
        string scriptPath,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(connection);

        if (!File.Exists(scriptPath))
        {
            throw new FileNotFoundException("The SQL script was not found.", scriptPath);
        }

        var script = await File.ReadAllTextAsync(scriptPath);
        var statements = SplitStatements(script);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var statement in statements)
            {
                await using var command = new MySqlCommand(statement, connection, transaction);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            return statements.Count;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static List<string> SplitStatements(string script)
    {
        var statements = new List<string>();
        var statement = new System.Text.StringBuilder();
        var quote = '\0';
        var inLineComment = false;
        var inBlockComment = false;

        for (var index = 0; index < script.Length; index++)
        {
            var character = script[index];
            var nextCharacter = index + 1 < script.Length ? script[index + 1] : '\0';

            if (inLineComment)
            {
                if (character is '\r' or '\n')
                {
                    inLineComment = false;
                    statement.Append(character);
                }

                continue;
            }

            if (inBlockComment)
            {
                if (character == '*' && nextCharacter == '/')
                {
                    inBlockComment = false;
                    index++;
                }

                continue;
            }

            if (quote == '\0' && character == '-' && nextCharacter == '-')
            {
                inLineComment = true;
                index++;
                continue;
            }

            if (quote == '\0' && character == '#')
            {
                inLineComment = true;
                continue;
            }

            if (quote == '\0' && character == '/' && nextCharacter == '*')
            {
                inBlockComment = true;
                index++;
                continue;
            }

            if (character is '\'' or '"' or '`')
            {
                if (quote == '\0')
                {
                    quote = character;
                }
                else if (quote == character && (index == 0 || script[index - 1] != '\\'))
                {
                    quote = '\0';
                }
            }

            if (character == ';' && quote == '\0')
            {
                AddStatement(statement, statements);
                continue;
            }

            statement.Append(character);
        }

        AddStatement(statement, statements);
        return statements;

        static void AddStatement(System.Text.StringBuilder builder, List<string> values)
        {
            var value = builder.ToString().Trim();
            if (value.Length > 0)
            {
                values.Add(value);
            }

            builder.Clear();
        }
    }
}