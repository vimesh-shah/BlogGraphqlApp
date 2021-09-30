using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Tags;

public class AddTagPayload: TagPayloadBase
{
    public AddTagPayload(Tag tag) : base(tag)
    { 
    }

    public AddTagPayload(IReadOnlyList<UserError> errors):base(errors)
    { 
    }
}