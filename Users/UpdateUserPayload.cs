using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Users;

public class UpdateUserPayload : UserPayloadBase
{
    public UpdateUserPayload(User user) : base(user)
    {
    }

    public UpdateUserPayload(IReadOnlyList<UserError> errors)
        : base(errors)
    {
    }
}