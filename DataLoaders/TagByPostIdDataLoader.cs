using BlogGraphqlApp.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.DataLoaders;

public class TagByPostIdDataLoader: GroupedDataLoader<int, Tag>
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;
    
    public TagByPostIdDataLoader(
        IDbContextFactory<BlogDbContext> dbContextFactory,
        IBatchScheduler batchScheduler, 
        DataLoaderOptions? options = null) 
        : base(batchScheduler, options)
    {
        _contextFactory = dbContextFactory;
    }

    protected override async Task<ILookup<int, Tag>> LoadGroupedBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var posts = await dbContext.Posts
            .Where(u => keys.Contains(u.Id))
            .Include(u => u.Tags)
            .ToListAsync(cancellationToken);

        var list = new List<Tuple<int, Tag>>();

        foreach (var post in posts)
        {
            foreach (var tag in post.Tags)
            {
                list.Add(Tuple.Create(post.Id, tag));
            }
        }

        return list.ToLookup(x => x.Item1, t => t.Item2);
    }
}