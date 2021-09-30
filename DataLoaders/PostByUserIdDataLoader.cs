using BlogGraphqlApp.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.DataLoaders;

public class PostByUserIdDataLoader : GroupedDataLoader<int, Post>
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;
    
    public PostByUserIdDataLoader(
        IDbContextFactory<BlogDbContext> dbContextFactory,
        IBatchScheduler batchScheduler, 
        DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _contextFactory = dbContextFactory;
    }

    protected override async Task<ILookup<int, Post>> LoadGroupedBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var users = await dbContext.Users
            .Where(u => keys.Contains(u.Id))
            .Include(u => u.Posts)
            .ToListAsync(cancellationToken);

        var list = new List<Tuple<int, Post>>();

        foreach (var user in users)
        {
            foreach (var post in user.Posts)
            {
                list.Add(Tuple.Create(user.Id, post));
            }
        }

        return list.ToLookup(x => x.Item1, t => t.Item2);
    }
}