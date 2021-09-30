using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Tags;

public class TagPayloadBase: Payload
{
    public Tag Tag { get; }
    
    protected TagPayloadBase(Tag tag)
    {
        Tag = tag;
    }

    protected TagPayloadBase(IReadOnlyList<UserError> errors) : base(errors)
    {

    }
}