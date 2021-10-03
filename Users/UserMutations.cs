using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogGraphqlApp.Common;
using BlogGraphqlApp.Data;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlogGraphqlApp.Users;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class UserMutations
{
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<AddUserPayload> AddUserAsync(
        AddUserInput input,
        [ScopedService] BlogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = input.Username,
            Password = input.Password,
            Firstname = input.Firstname,
            Lastname = input.Lastname,
            Age = input.Age,
            Gender = input.Gender
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddUserPayload(user);
    }

    [UseDbContext(typeof(BlogDbContext))]
    public async Task<LoginPayload> LoginAsync(
        LoginInput input,
        [ScopedService] BlogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
                                   .AsNoTracking()
                                   .Where(x => x.Username!.Equals(input.Username) &&
                                               x.Password!.Equals(input.Password))
                                   .FirstOrDefaultAsync();

        if (user is null)
        {
            return new LoginPayload(new List<UserError?>{
                new UserError{
                    Message = "Username or password may be incorrect",
                    Code = "Unauthenticated"
                }
            });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("MySuperSecretKey");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "https://auth.chillicream.com",
            Audience = "https://graphql.chillicream.com",
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var tokenString = tokenHandler.WriteToken(token);

        return new LoginPayload(user, tokenString);
    }

    //[Authorize]
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<UpdateUserPayload> UpdateUserAsync(
        UpdateUserInput input,
        ClaimsPrincipal claimsPrincipal,
        [ScopedService] BlogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var userIdClaim = claimsPrincipal.Claims.Where(x => x.Type.Equals("id")).FirstOrDefault();

        var user = await dbContext.Users
            .Where(x => x.Id == int.Parse(userIdClaim.Value))
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return new UpdateUserPayload(new List<UserError>{
                new UserError{
                    Message = "Specified user is not exits",
                    Code = "USER_NOT_FOUND"
                }
            });
        }

        user.Firstname = input.Firstname;
        user.Lastname = input.Lastname;
        user.Age = input.Age;
        user.Gender = input.Gender;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateUserPayload(user);
    }

    //[Authorize]
    [UseDbContext(typeof(BlogDbContext))]
    public async Task<ChangePasswordPayload> ChangePasswordAsync(
        ChangePasswordInput input,
        ClaimsPrincipal claimsPrincipal,
        [ScopedService] BlogDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var userIdClaim = claimsPrincipal.Claims.Where(x => x.Type.Equals("id")).FirstOrDefault();

        var user = await dbContext.Users
            .Where(x => x.Id == int.Parse(userIdClaim.Value) &&
                        x.Password.Equals(input.OldPassword))
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return new ChangePasswordPayload(new List<UserError>{
                new UserError{
                    Message = "Specified user is not exits or old password is incorrect",
                    Code = "USER_NOT_FOUND_OR_PASSWORD_IS_INCORRECT"
                }
            });
        }

        user.Password = input.NewPassword;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new ChangePasswordPayload(user);
    }
}