using System.Threading;
using System.Threading.Tasks;
using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using HotChocolate;
using HotChocolate.Types;

namespace BlogGraphqlApp.Posts;

[ExtendObjectType(OperationTypeNames.Subscription)]
public class PostSubscriptions
{
    [Subscribe]
    [Topic]
    public Task<Post> OnPostAddedAsync(
        [EventMessage] int postId,
        PostByIdDataLoader postById,
        CancellationToken cancellationToken)
    {
        return postById.LoadAsync(postId, cancellationToken);
    }

}
