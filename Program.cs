using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using BlogGraphqlApp.Posts;
using BlogGraphqlApp.Tags;
using BlogGraphqlApp.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPooledDbContextFactory<BlogDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.UseLoggerFactory(LoggerFactory.Create(x => { x.AddConsole(); }));
        options.EnableSensitiveDataLogging();
    })
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    
    .AddTypeExtension<PostQueries>()
    .AddTypeExtension<PostMutations>()
    .AddTypeExtension<PostNode>()
    .AddDataLoader<PostByIdDataLoader>()
    .AddDataLoader<PostByUserIdDataLoader>()
    
    .AddTypeExtension<UserQueries>()
    .AddTypeExtension<UserMutations>()
    .AddTypeExtension<UserNode>()
    .AddTypeExtension<UserByIdDataLoader>()
    
    .AddTypeExtension<TagQueries>()
    .AddTypeExtension<TagMutations>()
    .AddTypeExtension<TagNode>()
    .AddTypeExtension<TagByIdDataLoader>()
    .AddTypeExtension<TagByPostIdDataLoader>();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("/graphql", true);
        return Task.CompletedTask;
    });
});

app.Run();