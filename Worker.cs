using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Dapper;

namespace AzureWorkerDemoNET6
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly TelemetryClient telemetryClient;
        private readonly IHostApplicationLifetime appLifetime;
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient = new HttpClient();

        public Worker(
            ILogger<Worker> logger, 
            IHostApplicationLifetime appLifetime, 
            TelemetryClient telemetryClient,
            IConfiguration configuration )
        {
            this.logger = logger;
            this.telemetryClient = telemetryClient;
            this.configuration = configuration;
            this.appLifetime = appLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var request = telemetryClient.StartOperation<RequestTelemetry>("Execute"))
            {
                try
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    using var connection = new SqlConnection(configuration.GetConnectionString("db"));
                    await connection.OpenAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    telemetryClient.TrackException(ex);
                    request.Telemetry.ResponseCode = "Fail";
                    request.Telemetry.Success = false;
                }
            }

            appLifetime.StopApplication();
        }
    }
}