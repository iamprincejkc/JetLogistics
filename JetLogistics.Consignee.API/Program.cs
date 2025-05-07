using System.Reflection;
using JetLogistics.Consignee.API.Common;
using JetLogistics.Consignee.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

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


var configuration = builder.Configuration;
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = configuration["JWT:Authority"];
        options.RequireHttpsMetadata = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidateAudience = true, 
            ValidAudience =  "consignee_audience",
            RequireExpirationTime = true,
            ValidateLifetime = true, 
            ClockSkew = TimeSpan.FromMinutes(int.TryParse(configuration["JWT:ClockSkew"], out var skew) ? skew : 2),

        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse(); // suppress default behavior
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return context.Response.WriteAsync("Token is missing or invalid.");
            }
        };
    });



// Register 
builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddScoped<IConsigneeService, ConsigneeService>();


// Auto-register all ICommandHandler and IQueryHandler
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
    app.UseSwagger(opt =>
    {
        opt.RouteTemplate = "openapi/{documentName}.json";
    });



    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("JET LOGISTICS CONSIGNEE API")
        .WithSidebar(true)
        .WithTheme(ScalarTheme.Mars)
        .WithDarkModeToggle(true);
        options.AddHttpAuthentication("Bearer", bearer =>
        {
            bearer.Token = "your-bearer-token";
        });
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
