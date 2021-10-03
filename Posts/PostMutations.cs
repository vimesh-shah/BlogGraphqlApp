using System.Security.Claims;
using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.Posts;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class PostMutations
{
    // [Authorize]
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<AddPostPayload> AddPostAsync(
        AddPostInput input,
        ClaimsPrincipal claimsPrincipal,
        [ScopedService] BlogDbContext dbContext,
        [Service] ITopicEventSender eventSender,
        CancellationToken cancellationToken)
    {
        var post = new Post
        {
            Title = input.Title,
            Content = input.Content,
            CreatedAt = DateTime.UtcNow
        };

        var tags = await dbContext.Tags
            .Where(x => input.Tags.Contains(x.Name!))
            .ToListAsync();

        foreach (var tag in input.Tags)
        {
            if (tags.Any(x => x.Name.Equals(tag)) == false)
            {
                var newTag = new Tag
                {
                    Name = tag
                };

                dbContext.Tags.Add(newTag);
                post.Tags.Add(newTag);
            }
        }

        var userIdClaim = claimsPrincipal.Claims.Where(x => x.Type.Equals("id")).FirstOrDefault();

        var user = await dbContext.Users
            .Where(x => x.Id == int.Parse(userIdClaim.Value))
            .FirstOrDefaultAsync();

        post.Tags.AddRange(tags);
        post.CreatedBy = user;
        dbContext.Posts.Add(post);

        await dbContext.SaveChangesAsync(cancellationToken);

        await eventSender.SendAsync(
            nameof(PostSubscriptions.OnPostAddedAsync),
            post.Id
        );

        return new AddPostPayload(post);
    }
}