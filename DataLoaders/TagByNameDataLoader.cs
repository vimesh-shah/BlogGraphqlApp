using BlogGraphqlApp.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.DataLoaders;

public class TagByNameDataLoader : BatchDataLoader<string, Tag>
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;

    public TagByNameDataLoader(
        IDbContextFactory<BlogDbContext> contextFactory,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null) :
        base(batchScheduler, options)
    {
        _contextFactory = contextFactory;
    }

    protected override async Task<IReadOnlyDictionary<string, Tag>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        return await dbContext.Tags
            .AsNoTracking()
            .Where(p => keys.Contains(p.Name!))
            .ToDictionaryAsync(p => p.Name!, cancellationToken);
    }
}
