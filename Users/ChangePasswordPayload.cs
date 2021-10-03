using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Users;

public class ChangePasswordPayload : UserPayloadBase
{
    public ChangePasswordPayload(User user) : base(user)
    {
    }

    public ChangePasswordPayload(IReadOnlyList<UserError> errors)
        : base(errors)
    {
    }
}