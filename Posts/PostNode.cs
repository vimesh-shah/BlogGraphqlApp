using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace BlogGraphqlApp.Posts;

[Node]
[ExtendObjectType(typeof(Post))]
public class PostNode
{
    [BindMember(nameof(Post.Tags), Replace = true)]
    public async Task<IEnumerable<Tag>> GetTagsAsync(
        [Parent] Post post,
        TagByPostIdDataLoader tagByPostIdDataLoader,
        CancellationToken cancellationToken)
    {
        return await tagByPostIdDataLoader.LoadAsync(post.Id, cancellationToken);
    }
    
    [BindMember(nameof(Post.CreatedBy), Replace = true)]
    public async Task<User> GetCreatedByAsync(
        [Parent] Post post,
        UserByPostIdDataLoader userByPostId,
        CancellationToken cancellationToken)
    {
        var data = await userByPostId.LoadAsync(post.Id, cancellationToken);
        return data;
    }

    [NodeResolver]
    public static Task<Post> GetPostByIdAsync(
        int id,
        PostByIdDataLoader postById,
        CancellationToken cancellationToken)
    {
        return postById.LoadAsync(id, cancellationToken);
    }
}