using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.Tags;

[ExtendObjectType(OperationTypeNames.Query)]
public class TagQueries
{
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<IEnumerable<Tag>> GetTags([ScopedService] BlogDbContext dbContext)
    {
        return await dbContext.Tags.ToListAsync();
    }

    public async Task<Tag> GetTagById(
        [ID(nameof(Tag))] int id,
        TagByIdDataLoader tagById,
        CancellationToken cancellationToken)
    {
        return await tagById.LoadAsync(id, cancellationToken);
    }
}