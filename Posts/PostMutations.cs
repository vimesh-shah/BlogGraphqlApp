using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.Posts;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class PostMutations
{
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<AddPostPayload> AddPostAsync(
        AddPostInput input, 
        [ScopedService] BlogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var post = new Post
        {
            Title = input.Title,
            Content = input.Content,
            CreatedAt = DateTime.UtcNow
        };

        var tags = await dbContext.Tags
            .Where(x => input.Tags.Contains(x.Name))
            .ToListAsync();
        
        var user = await dbContext.Users
            .Where(x => x.Username == input.CreatedBy)
            .FirstOrDefaultAsync();

        post.Tags.AddRange(tags);
        post.CreatedBy = user;
        dbContext.Posts.Add(post);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddPostPayload(post);
    }
}