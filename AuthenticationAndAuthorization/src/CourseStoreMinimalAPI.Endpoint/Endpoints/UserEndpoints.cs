using AutoMapper;
using CourseStoreMinimalAPI.ApplicationServices;
using CourseStoreMinimalAPI.Endpoint.Infrastructures;
using CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;
using CourseStoreMinimalAPI.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class UserEndpoints
{
    static string CacheKey = "users";
    public static WebApplication MapUsers(this WebApplication app, string prefix)
    {
        var categoryGroup = app.MapGroup(prefix);
        categoryGroup.MapPost("/", Insert);//.AddEndpointFilter<ValidationFilter<CategoryRequest>>();
        categoryGroup.MapPost("/Login", Login);
        return app;
    }

    static async Task<Results<Created<UserRegistrationResponse>, BadRequest<IEnumerable<IdentityError>>>> Insert(
                                                        [FromServices] UserManager<IdentityUser> userManager,
                                                        IMapper mapper,
                                                        UserRegistrationRequest registrationRequest)
    {
        var IdentityUser = new IdentityUser(registrationRequest.Email);

        var resutlt = await userManager.CreateAsync(IdentityUser, registrationRequest.Password);
        if (resutlt.Succeeded)
        {
            var response = new UserRegistrationResponse { IsOk = true };


            return TypedResults.Created($"/users/{IdentityUser.Id}", response);
        }
        return TypedResults.BadRequest<IEnumerable<IdentityError>>(resutlt.Errors);
    }

    static async Task<Results<Ok<UserLoginResponse>, BadRequest>> Login(
                                                        [FromServices] UserManager<IdentityUser> userManager,
                                                        IMapper mapper,
                                                        UserLoginRequest loginRequest)
    {

        var IdentityUser = await userManager.FindByNameAsync(loginRequest.Email);
        if (IdentityUser == null)
        {
            return TypedResults.BadRequest();
        }
        bool isValidPassword = await userManager.CheckPasswordAsync(IdentityUser, loginRequest.Password);
        if (!isValidPassword)
        {
            return TypedResults.BadRequest();
        }
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, IdentityUser.UserName),
            new(ClaimTypes.Country, "Iran"),
            new("Oranization","Nikamooz")

        };
        var authSiningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("mx9M34DBulDGxE4WL6t9IHPaHZ1j7fcLkYahrSgHZf4="));

        var token = new JwtSecurityToken(
            issuer: "CourseStoreMinimalAPI",
            audience: "CourseStoreMinimalAPI",
            expires: DateTime.Now.AddHours(2),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSiningKey, SecurityAlgorithms.HmacSha256));

        return TypedResults.Ok(new UserLoginResponse
        {
            JWT = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        });
    }
}
