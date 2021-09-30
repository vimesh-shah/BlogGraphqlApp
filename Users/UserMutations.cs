using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace BlogGraphqlApp.Users;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class UserMutations
{
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<AddUserPayload> AddUserAsync(
        AddUserInput input,
        [ScopedService] BlogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = input.Username,
            Firstname = input.Firstname,
            Lastname = input.Lastname,
            Age = input.Age,
            Gender = input.Gender
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddUserPayload(user);
    }
}