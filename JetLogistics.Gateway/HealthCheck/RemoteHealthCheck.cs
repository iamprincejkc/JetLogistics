using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JetLogistics.Gateway.HealthCheck
{
    public class RemoteHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public RemoteHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var url = _configuration["RemoteFileCheck:Url"]; // e.g. https://ftp.jetlogistics.co.jp/Checklist/Vehicle/sample/sample.jpg
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync(url, cancellationToken);
                return response.IsSuccessStatusCode
                    ? HealthCheckResult.Healthy("Remote file is reachable.")
                    : HealthCheckResult.Unhealthy($"Remote file returned status code {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Remote file check failed: {ex.Message}");
            }
        }
    }
}
