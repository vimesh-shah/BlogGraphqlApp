using BlogGraphqlApp.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.DataLoaders;

public class UserByIdDataLoader : BatchDataLoader<int, User>
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;

    public UserByIdDataLoader(
        IDbContextFactory<BlogDbContext> contextFactory,
        IBatchScheduler batchScheduler, 
        DataLoaderOptions? options = null) : 
        base(batchScheduler, options)
    {
        _contextFactory = contextFactory;
    }

    protected override async Task<IReadOnlyDictionary<int, User>> LoadBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        return await dbContext.Users
            .AsNoTracking()
            .Where(p => keys.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
    }
}  