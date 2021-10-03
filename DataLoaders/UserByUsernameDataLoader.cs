using BlogGraphqlApp.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.DataLoaders;

public class UserByUsernameDataLoader : BatchDataLoader<string, User>
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;

    public UserByUsernameDataLoader(
        IDbContextFactory<BlogDbContext> contextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null) :
        base(batchScheduler, options)
    {
        _contextFactory = contextFactory;
    }

    protected override async Task<IReadOnlyDictionary<string, User>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        return await dbContext.Users
            .AsNoTracking()
            .Where(p => keys.Contains(p.Username))
            .ToDictionaryAsync(p => p.Username!, cancellationToken);
    }
}
