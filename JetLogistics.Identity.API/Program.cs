using System.Reflection;
using HealthChecks.UI.Client;
using JetLogistics.Identity.API.Common;
using JetLogistics.Identity.API.DataAccess;
using JetLogistics.Identity.API.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "JET LOGISTICS IDENTITY API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token. Example: Bearer {token}"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseInMemoryDatabase("IdentityDb");
    options.UseOpenIddict();
});

builder.Services.AddOpenIddict()
    .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<IdentityDbContext>())
    .AddServer(options =>
    {
        options.AllowPasswordFlow();
        options.AllowRefreshTokenFlow();
        options.SetTokenEndpointUris("/connect/token");
        options.AcceptAnonymousClients();
        options.AddDevelopmentEncryptionCertificate();
        options.DisableAccessTokenEncryption();
        options.AddDevelopmentSigningCertificate();
        options.UseAspNetCore().EnableTokenEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireGatewayScope", policy =>
    {
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "scope" && c.Value.Split(' ').Contains("sc1")));
    });
});

builder.Services.AddOpenIddict().AddValidation(options =>
{
    options.UseLocalServer();
    options.UseAspNetCore();
});

builder.Services.AddScoped<IDispatcher, Dispatcher>();

builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime())
    
    .Scan(scan => scan
    .FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(opt => { opt.RouteTemplate = "openapi/{documentName}.json"; });
    app.MapScalarApiReference(options =>
    {
        options.WithSidebar(true).WithTheme(ScalarTheme.Mars).WithDarkModeToggle(true);
        options.AddHttpAuthentication("Bearer", bearer => { bearer.Token = "your-bearer-token"; });
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await OpenIddictSeeder.SeedAsync(app.Services);
app.Run();
