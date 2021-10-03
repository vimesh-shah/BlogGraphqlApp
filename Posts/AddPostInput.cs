using BlogGraphqlApp.Data;

namespace BlogGraphqlApp.Posts;

public class AddPostInput
{
    public string? Title { get; set; }

    public string? Content { get; set; }

    public List<string> Tags { get; set; } = new List<string>();
}