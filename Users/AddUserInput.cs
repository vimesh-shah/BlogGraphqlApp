using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Users;

public class AddUserInput
{
    public string? Username { get; set; }
    
    public string? Firstname { get; set; }
    
    public string? Lastname { get; set; }
    
    public short Age { get; set; }
    
    public Gender Gender { get; set; }
}