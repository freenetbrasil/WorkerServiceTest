using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace WorkerServiceTest
{
    public class ServiceManagerConsumer : IConsumer<ServiceManagerRequest>
    {
        private readonly ILogger<ServiceManagerConsumer> _logger;

        public ServiceManagerConsumer(ILogger<ServiceManagerConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ServiceManagerRequest> context)
        {
            _logger.LogInformation("Received Text: {Text}", context.Message.Action);

            await context.RespondAsync<ServiceManagerResult>(new ServiceManagerResult()
            {
                Status = $"Ok - {DateTime.Now}"
            });;
        }
    }
}
