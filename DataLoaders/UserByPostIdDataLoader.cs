using BlogGraphqlApp.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.DataLoaders;

public class UserByPostIdDataLoader : BatchDataLoader<int, User>
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;

    public UserByPostIdDataLoader(
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
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        
        var posts = await dbContext.Posts
            .AsNoTracking()
            .Where(x => keys.Contains(x.Id))
            .Include(x => x.CreatedBy)
            .ToListAsync(cancellationToken);

        var dict = new Dictionary<int, User>();
        
        foreach (var post in posts)
        {
            dict.Add(post.Id, post.CreatedBy);
        }

        return dict;
    }
}