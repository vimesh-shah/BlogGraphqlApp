using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Users
{
    public class LoginPayload : UserPayloadBase
    {
        public string? Token { get; set; }

        public LoginPayload(User user, string? token) : base(user)
        {
            Token = token;
        }

        public LoginPayload(IReadOnlyList<UserError?> errors) : base(errors!)
        {

        }
    }
}