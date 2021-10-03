using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.Posts;

[ExtendObjectType(OperationTypeNames.Query)]
public class PostQueries
{
    // [Authorize]
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<IEnumerable<Post>> GetPosts([ScopedService] BlogDbContext dbContext)
    {
        return await dbContext.Posts.ToListAsync();
    }

    // [Authorize]
    public async Task<Post> GetPostById(
        [ID(nameof(Post))] int id,
        PostByIdDataLoader postById,
        CancellationToken cancellationToken)
    {
        return await postById.LoadAsync(id, cancellationToken);
    }
}