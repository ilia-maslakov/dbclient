using MassTransit;
using System;
using System.Threading.Tasks;
using Stkpnt.Contracts;
using Microsoft.Extensions.Logging;

namespace MassTransit
{
    public class ApplicationUserAddConsumer : IConsumer<IApplicationUserAdd>
    {
        private readonly ILogger<ApplicationUserAddConsumer> _logger;

        public ApplicationUserAddConsumer(ILogger<ApplicationUserAddConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<IApplicationUserAdd> context)
        {
            _logger.LogWarning($"public Task Consume");
            return Task.CompletedTask;
        }
    }
}
