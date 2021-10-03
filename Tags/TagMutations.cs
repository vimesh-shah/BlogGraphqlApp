using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

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

    [UseDbContext(typeof(BlogDbContext))]
    public async Task<RenameTagPayload> RenameTagAsync(
        RenameTagInput input,
        [ScopedService] BlogDbContext dbContext,
        CancellationToken cancellationToken)
    {

        var tag = await dbContext.Tags.Where(x => x.Name.Equals(input.OldName)).FirstOrDefaultAsync();

        if (tag is null)
        {
            return new RenameTagPayload(new List<UserError>{
                new UserError{
                    Message = "Specified tag is not exits",
                    Code = "TAG_NOT_FOUND"
                }
            });
        }

        tag.Name = input.NewName;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new RenameTagPayload(tag);
    }
}