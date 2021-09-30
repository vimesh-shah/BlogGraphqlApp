using BlogGraphqlApp.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.DataLoaders;

public class PostByTagIdDataLoader: GroupedDataLoader<int, Post>
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;
    
    public PostByTagIdDataLoader(
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

        var tags = await dbContext.Tags
            .Where(u => keys.Contains(u.Id))
            .Include(u => u.Posts)
            .ToListAsync(cancellationToken);

        var list = new List<Tuple<int, Post>>();

        foreach (var tag in tags)
        {
            foreach (var post in tag.Posts)
            {
                list.Add(Tuple.Create(tag.Id, post));
            }
        }

        return list.ToLookup(x => x.Item1, t => t.Item2);
    }
}