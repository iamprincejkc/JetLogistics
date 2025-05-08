using System.Security.Claims;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using JetLogistics.Gateway.Extensions;
using JetLogistics.Gateway.HealthCheck;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);





var configuration = builder.Configuration;
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7001"; // IdentityAPI
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = "gateway",
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(int.TryParse(configuration["JWT:ClockSkew"], out var skew) ? skew : 2),
        };

    });


builder.Services.AddOcelot();


builder.Services.ConfigureHealthChecks(builder.Configuration);


var app = builder.Build();



// JSON endpoint for health check results
app.MapWhen(context => context.Request.Path.StartsWithSegments("/internal-health"), subApp =>
{
    subApp.UseHealthChecks("/internal-health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});


// HealthChecks UI (browser-friendly dashboard)
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/healthcheck-ui"; // DIFFERENT from above
    options.AddCustomStylesheet("./HealthCheck/customhealth.css");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();



app.UseAuthentication();
app.UseAuthorization();


//this should be before app.useocelot to properly create the scope for ocelot compatibility
//custom middleware
app.UseScopeClaimSplitter();

app.UseOcelot().Wait();

app.MapControllers();




app.Run();
