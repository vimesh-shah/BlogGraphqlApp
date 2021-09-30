using BlogGraphqlApp.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.DataLoaders;

public class PostByIdDataLoader : BatchDataLoader<int, Post>
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;

    public PostByIdDataLoader(
        IDbContextFactory<BlogDbContext> contextFactory,
        IBatchScheduler batchScheduler, 
        DataLoaderOptions? options = null) : 
        base(batchScheduler, options)
    {
        _contextFactory = contextFactory;
    }

    protected override async Task<IReadOnlyDictionary<int, Post>> LoadBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using var dbContext = _contextFactory.CreateDbContext();

        return await dbContext.Posts
            .AsNoTracking()
            .Where(p => keys.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
    }
}