using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace HelloDotnet5
{
    // Checks the health status of the OpenWeather service
    public class ExternalEndpointHealthCheck : IHealthCheck
    {
        private readonly ServiceSettings _settings;

        public ExternalEndpointHealthCheck(IOptions<ServiceSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Ping ping = new(); // dotnet 5.0 new features declaring a new type...
            var reply = await ping.SendPingAsync(_settings.OpenWeatherHost);

            if (reply.Status != IPStatus.Success)
            {
                return HealthCheckResult.Unhealthy();
            }

            return HealthCheckResult.Healthy();
        }
    }
}