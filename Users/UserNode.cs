using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace BlogGraphqlApp.Users;

[Node]
[ExtendObjectType(typeof(User))]
public class UserNode
{
    [BindMember(nameof(User.Posts), Replace = true)]
    public async Task<IEnumerable<Post>> GetPostsAsync(
        [Parent] User user,
        PostByUserIdDataLoader postByUserIdDataLoader,
        CancellationToken cancellationToken)
    {
        return await postByUserIdDataLoader.LoadAsync(user.Id, cancellationToken);
    }

    [NodeResolver]
    public static Task<User> GetUserByIdAsync(
        int id,
        UserByIdDataLoader userById,
        CancellationToken cancellationToken)
    {
        return userById.LoadAsync(id, cancellationToken);
    }
}