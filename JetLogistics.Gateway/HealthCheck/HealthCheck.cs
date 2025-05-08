using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JetLogistics.Gateway.HealthCheck
{
    public static class HealthCheck
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration["ConnectionStrings:DefaultConnection"], healthQuery: "select 1", name: "SQL Server", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Database" })
                .AddCheck<RemoteHealthCheck>("FTP", failureStatus: HealthStatus.Unhealthy, tags: new[] {"FTP" })
                .AddUrlGroup(new Uri("https://localhost:7001/health"), name: "Identity API", tags: new[] { "Identity","API" } )
                .AddUrlGroup(new Uri("https://localhost:7017/health"), name: "Consignee API", tags: new[] { "Consignee", "API" })
                .AddUrlGroup(new Uri("https://localhost:7217/health"), name: "Booking API", tags: new[] { "Booking", "API" })


                .AddUrlGroup(new Uri("https://localhost:7260/identityhealth"), name: "Identity API Gateway", tags: new[] { "Gateway" ,"Identity", "API" })
                .AddUrlGroup(new Uri("https://localhost:7260/consigneehealth"), name: "Consignee API Gateway", tags: new[] { "Gateway", "Consignee", "API" })
                .AddUrlGroup(new Uri("https://localhost:7260/bookinghealth"), name: "Booking API Gateway", tags: new[] { "Gateway", "Booking", "API" });

            //services.AddHealthChecksUI();
            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10); //time in seconds between check    
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.AddHealthCheckEndpoint("JET LOGISTICS HEALTH CHECK", "/internal-health"); //map health check api    

            })
                .AddInMemoryStorage();
        }
    }
}
