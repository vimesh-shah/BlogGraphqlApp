using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.Users;

[ExtendObjectType(OperationTypeNames.Query)]
public class UserQueries
{
    //[Authorize]
    [UseDbContext(typeof(BlogDbContext))]
    public IEnumerable<User> GetUsers([ScopedService] BlogDbContext dbContext)
    {
        return dbContext.Users;
    }

    //[Authorize]
    public async Task<User> GetUserById(
        [ID(nameof(User))] int id,
        UserByIdDataLoader userById,
        CancellationToken cancellationToken)
    {
        return await userById.LoadAsync(id, cancellationToken);
    }

    //[Authorize]
    public async Task<User> GetUserByUsername(
        string? username,
        UserByUsernameDataLoader userByUsername,
        CancellationToken cancellationToken
    )
    {
        return await userByUsername.LoadAsync(username!, cancellationToken);
    }

}