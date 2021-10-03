using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Tags;

public class RenameTagPayload : TagPayloadBase
{
    public RenameTagPayload(Tag tag) : base(tag)
    {
    }

    public RenameTagPayload(IReadOnlyList<UserError> errors) : base(errors)
    {
    }
}