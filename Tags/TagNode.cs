using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace BlogGraphqlApp.Tags;

[Node]
[ExtendObjectType(typeof(Tag))]
public class TagNode
{
    [BindMember(nameof(Tag.Posts), Replace = true)]
    public async Task<IEnumerable<Post>> GetPostsAsync(
        [Parent] Tag tag,
        PostByTagIdDataLoader tagByPostIdDataLoader,
        CancellationToken cancellationToken)
    {
        return await tagByPostIdDataLoader.LoadAsync(tag.Id, cancellationToken);
    }

    [NodeResolver]
    public static Task<Tag> GetTagByIdAsync(
        int id,
        TagByIdDataLoader tagById,
        CancellationToken cancellationToken)
    {
        return tagById.LoadAsync(id, cancellationToken);
    }
}