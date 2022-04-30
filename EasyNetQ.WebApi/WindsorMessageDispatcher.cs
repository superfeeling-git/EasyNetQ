using EasyNetQ.AutoSubscribe;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EasyNetQ.WebApi
{
    public class WindsorMessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IServiceProvider service;

        public WindsorMessageDispatcher(IServiceProvider service)
        {
            this.service = service;
        }

        void IAutoSubscriberMessageDispatcher.Dispatch<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            var consumer = service.GetService<IConsume<MessageDto>>();
            consumer.Consume(message as MessageDto, cancellationToken);
        }

        async Task IAutoSubscriberMessageDispatcher.DispatchAsync<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            var consumer = service.GetService<TConsumer>();
            await consumer.ConsumeAsync(message, cancellationToken);
        }
    }
}
