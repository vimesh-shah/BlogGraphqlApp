using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Posts;

public class AddPostPayload: PostPayloadBase
{
    public AddPostPayload(Post post) : base(post)
    {
        
    }

    public AddPostPayload(IReadOnlyList<UserError> errors) : base(errors)
    {
        
    }
}