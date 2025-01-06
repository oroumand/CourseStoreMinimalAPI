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

namespace CourseStoreMinimalAPI.Endpoint.Endpoints;

public static class UserEndpoints
{
    static string CacheKey = "users";
    public static WebApplication MapUsers(this WebApplication app, string prefix)
    {
        var categoryGroup = app.MapGroup(prefix);
        categoryGroup.MapPost("/", Insert);//.AddEndpointFilter<ValidationFilter<CategoryRequest>>();

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


}
