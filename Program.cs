using AzureWorkerDemoNET6;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.ApplicationInsights.Extensibility;
using Azure.Identity;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureLogging(logging => logging.AddApplicationInsights());

builder.ConfigureAppConfiguration(config =>
{
    var settings = config.Build();
    var keyvaultUri = settings.GetSection("AppSettings").GetValue<string>("KEY_VAULT_URI");
    if ( !string.IsNullOrWhiteSpace( keyvaultUri ))
        config.AddAzureKeyVault(new Uri(keyvaultUri), new DefaultAzureCredential());
});

builder.ConfigureServices(services =>
{
    services.AddHostedService<Worker>();
    var options = new ApplicationInsightsServiceOptions() {         
        EnableAdaptiveSampling = false, // infrequently run command line tools should record all events
    };
    services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();
    services.AddApplicationInsightsTelemetryWorkerService( options );
});

var host = builder.Build();    

await host.RunAsync();
