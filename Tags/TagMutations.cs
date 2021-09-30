using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace BlogGraphqlApp.Tags;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class TagMutations
{
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<AddTagPayload> AddTagAsync(
        AddTagInput input,
        [ScopedService] BlogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var tag = new Tag
        {
            Name = input.Name
        };

        dbContext.Tags.Add(tag);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new AddTagPayload(tag);
    }
}