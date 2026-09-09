using System.Data;

namespace Zen.Libraries.Data;

public interface IDapperContext
{
    Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<T?> ExecuteScalarAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<TResult> WithTransactionAsync<TResult>(
        Func<IDbConnection, IDbTransaction, Task<TResult>> action,
        CancellationToken cancellationToken = default);
}