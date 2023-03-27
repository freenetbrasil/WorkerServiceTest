namespace ServiceClient;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DefaultScopedProcessingService> _logger;

    public Worker(
        IServiceProvider serviceProvider,
        ILogger<DefaultScopedProcessingService> logger) =>
        (_serviceProvider, _logger) = (serviceProvider, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                $"{nameof(DefaultScopedProcessingService)} is working.");

            using var scope = _serviceProvider.CreateScope();
            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

            await scopedProcessingService.DoWorkAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(DefaultScopedProcessingService)} is stopping.");

        await base.StopAsync(stoppingToken);
    }
}