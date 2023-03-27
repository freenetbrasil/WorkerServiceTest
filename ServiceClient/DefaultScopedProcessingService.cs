using Contracts;
using MassTransit;

namespace ServiceClient;

public sealed class DefaultScopedProcessingService : IScopedProcessingService
{
    private int _executionCount;
    private readonly ILogger<DefaultScopedProcessingService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DefaultScopedProcessingService(
        ILogger<DefaultScopedProcessingService> logger, IServiceProvider provider)
    {
        _logger = logger;
        _serviceProvider = provider;
    }

    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            ++ _executionCount;

            // Disable the comment below and it works
            //await Task.Delay(2000, stoppingToken);

            try
            {
                _logger.LogInformation(
                    "{ServiceName} working, execution count: {Count}",
                    nameof(DefaultScopedProcessingService),
                    _executionCount);
                var client = _serviceProvider.GetRequiredService<IRequestClient<ServiceManagerRequest>>();

                var response =
                    await client.GetResponse<ServiceManagerResult>(
                        new ServiceManagerRequest { Action = $"OK - {DateTime.Now}" }, stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}