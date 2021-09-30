using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Users;

public class AddUserPayload : UserPayloadBase
{
    public AddUserPayload(User user) : base(user)
    {
    }

    public AddUserPayload(IReadOnlyList<UserError> errors)
        : base(errors)
    {
    }
}