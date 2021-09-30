using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Posts;

public class PostPayloadBase: Payload
{
    public Post Post { get; }
    
    protected PostPayloadBase(Post post)
    {
        Post = post;
    }

    protected PostPayloadBase(IReadOnlyList<UserError> errors)
        :base(errors)
    {
        
    }
    
}