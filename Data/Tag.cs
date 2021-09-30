using System.ComponentModel.DataAnnotations;

namespace BlogGraphqlApp.Data;

public class Tag
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(20)]
    public string? Name { get; set; }

    public List<Post> Posts { get; set; } = new List<Post>();
}