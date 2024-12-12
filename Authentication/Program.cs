using Authentication.Infrastructure;
using Core.ASP.Net.Infrastructure.Config.Startup;
using Core.ASP.Net.Infrastructure.Filters;
using Core.ASP.Net.Infrastructure.Middlewares;
using Core.Contract.Config;
using Core.Contract.Errors;
using Core.Contract.Response;
using Core.Infrastructure.Config.Startup;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextServices(builder.Configuration);

AuthorizeConfigOptions authorizeConfig = builder.AddAuthorizeService();

builder.Services.AddRepositoryServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRequiredServices();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerService(authorizeConfig, "Auth Service");

builder.Services.AddControllers(option =>
{
    option.Filters.Add<ResourceFilter>();
    option.Filters.Add<AsyncResultFilter>();
    option.ModelValidatorProviders.Clear();
})
    .ConfigureApiBehaviorOptions(option =>
{
    option.SuppressModelStateInvalidFilter = false;

    option.InvalidModelStateResponseFactory = x =>
    {
        var errorValues = x.ModelState.Values.ToList();

        var errors = from item in x.ModelState
                     where item.Value.Errors.Count > 0
                     select new { item.Key, item.Value };

        var keys = errors.ToArray();

        return new BadRequestObjectResult(new Result(false, keys.Select(x => new Error("0", "Value Of Field Is Invalid.", x.Key.Replace("$", "").Replace(".", ""))).ToList()));
    };
})
    .AddControllersAsServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (authorizeConfig.AuthorizeType == "BasicAuthentication")
{
    app.UseMiddleware<BasicAuthorizationMiddleware>();
}

app.UseErrorHandler(app.Logger);

//app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.DocExpansion(DocExpansion.None);
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Auth V1");
    x.DocumentTitle = "احراز هویت";
    x.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
