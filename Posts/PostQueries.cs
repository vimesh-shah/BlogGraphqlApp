using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.Posts;

[ExtendObjectType(OperationTypeNames.Query)]
public class PostQueries
{
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<IEnumerable<Post>> GetPosts([ScopedService] BlogDbContext dbContext)
    {
        return await dbContext.Posts.ToListAsync();
    }
}