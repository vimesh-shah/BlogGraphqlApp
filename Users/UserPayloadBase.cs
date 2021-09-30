using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Users;

public class UserPayloadBase : Payload
{
    public User? User { get; }
    
    protected UserPayloadBase(User user)
    {
        User = user;
    }

    protected UserPayloadBase(IReadOnlyList<UserError> errors)
        : base(errors)
    {

    }
}