using RepoDb;

namespace Zen.Libraries.Data;

public sealed class RepoDbEntityStore<TEntity, TId>(IDbConnectionFactory connectionFactory)
    : IEntityStore<TEntity, TId>
    where TEntity : class
    where TId : notnull
{
    static RepoDbEntityStore()
    {
        GlobalConfiguration.Setup().UsePostgreSql();
    }

    public async Task<TEntity?> GetByIdAsync(
        TId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        return (await connection.QueryAsync<TEntity>(
            new { Id = id },
            cancellationToken: cancellationToken)).FirstOrDefault();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        return (await connection.QueryAllAsync<TEntity>(
            cancellationToken: cancellationToken)).ToList();
    }

    public async Task InsertAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await connection.InsertAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await connection.UpdateAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(
        TId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await connection.DeleteAsync<TEntity>(id, cancellationToken: cancellationToken);
    }
}