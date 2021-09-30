using Microsoft.EntityFrameworkCore;

namespace BlogGraphqlApp.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Post>()
            .HasOne(x => x.CreatedBy)
            .WithMany(x => x.Posts);

        modelBuilder
            .Entity<Post>()
            .HasMany(x => x.Tags)
            .WithMany(x => x.Posts);

        modelBuilder
            .Entity<Tag>()
            .HasMany(x => x.Posts)
            .WithMany(x => x.Tags);

        modelBuilder
            .Entity<Tag>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder
            .Entity<User>()
            .HasMany(x => x.Posts)
            .WithOne(x => x.CreatedBy);

        modelBuilder
            .Entity<User>()
            .HasIndex(x => x.Username)
            .IsUnique();
    }

    public DbSet<Post> Posts { get; set; } = default!;

    public DbSet<User> Users { get; set; } = default!;

    public DbSet<Tag> Tags { get; set; } = default!;
}