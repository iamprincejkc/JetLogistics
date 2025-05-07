using System.Reflection;
using JetLogistics.Identity.API.Common;
using JetLogistics.Identity.API.DataAccess;
using JetLogistics.Identity.API.Services;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Identity API", Version = "v1" });

    // ✅ Add JWT Authentication to Swagger
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
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<IdentityDbContext>();
    })
    .AddServer(options =>
    {
        options.AllowPasswordFlow();
        options.AllowRefreshTokenFlow();

        options.SetTokenEndpointUris("/connect/token");

        options.AcceptAnonymousClients();

        options.AddDevelopmentEncryptionCertificate();
        options.DisableAccessTokenEncryption();
        options.AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough();

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
            context.User.HasClaim(c =>
                c.Type == "scope" && c.Value.Split(' ').Contains("sc1")));
    });
});


builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });


// Register CQRS Dispatcher
builder.Services.AddScoped<IDispatcher, Dispatcher>();

// Auto-register command handlers
builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

// Auto-register query handlers
builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await OpenIddictSeeder.SeedAsync(app.Services);

app.Run();
