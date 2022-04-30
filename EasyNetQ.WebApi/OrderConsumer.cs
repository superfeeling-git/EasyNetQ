using EasyNetQ.AutoSubscribe;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyNetQ.WebApi
{
    public class OrderConsumer : IConsume<MessageDto>
    {
        private readonly ILogger<OrderConsumer> logger;

        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            this.logger = logger;
        }

        public void Consume(MessageDto message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(JsonConvert.SerializeObject(message));
        }
    }
}
