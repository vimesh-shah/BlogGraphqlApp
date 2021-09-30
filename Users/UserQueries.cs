using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.Users;

[ExtendObjectType(OperationTypeNames.Query)]
public class UserQueries
{
    [UseDbContext(typeof(BlogDbContext))]
    public IEnumerable<User> GetUsers([ScopedService] BlogDbContext dbContext)
    {
        return dbContext.Users;
    }

}