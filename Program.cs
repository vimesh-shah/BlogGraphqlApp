using System.Text;
using BlogGraphqlApp.Data;
using BlogGraphqlApp.DataLoaders;
using BlogGraphqlApp.Posts;
using BlogGraphqlApp.Tags;
using BlogGraphqlApp.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPooledDbContextFactory<BlogDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.UseLoggerFactory(LoggerFactory.Create(x => { x.AddConsole(); }));
        options.EnableSensitiveDataLogging();
    })
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType()
    .AddMutationType()
    .AddSubscriptionType()

    .AddTypeExtension<PostQueries>()
    .AddTypeExtension<PostMutations>()
    .AddTypeExtension<PostSubscriptions>()
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
    .AddTypeExtension<TagByPostIdDataLoader>()

    .AddInMemorySubscriptions();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://auth.chillicream.com",
            ValidAudience = "https://graphql.chillicream.com",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKey"))
        };
    });

builder.Services.AddAuthorization();

builder.Services
    .AddCors(o => o.AddDefaultPolicy(b => b
                                           .AllowAnyHeader()
                                           .AllowAnyMethod()
                                           .AllowAnyOrigin()));

var app = builder.Build();

app.UseCors();

app.UseWebSockets();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

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