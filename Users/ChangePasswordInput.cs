using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Users;

public class ChangePasswordInput
{
    public string? OldPassword { get; set; }

    public string? NewPassword { get; set; }
}