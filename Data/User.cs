using System.ComponentModel.DataAnnotations;

namespace BlogGraphqlApp.Data;

public enum Gender
{
    Male = 'M',
    Female = 'F'
}

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(20)]
    public string? Username { get; set; }
    
    [Required]
    [StringLength(50)]
    public string? Firstname { get; set; }
    
    [Required]
    [StringLength(50)]
    public string? Lastname { get; set; }
    
    [Required]
    public short Age { get; set; }
    
    [Required]
    public Gender Gender { get; set; }

    public List<Post> Posts { get; set; } = new List<Post>();
    
}