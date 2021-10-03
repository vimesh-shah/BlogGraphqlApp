using System.ComponentModel.DataAnnotations;
using HotChocolate.AspNetCore.Authorization;

namespace BlogGraphqlApp.Data;

public class Post
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string? Title { get; set; }

    [Required]
    [StringLength(1000)]
    public string? Content { get; set; }

    [Required]
    public List<Tag> Tags { get; set; } = new List<Tag>();

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public User? CreatedBy { get; set; }
}