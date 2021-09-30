using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
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
}