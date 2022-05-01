using EasyNetQ.AutoSubscribe;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EasyNetQ.WebApi
{
    /// <summary>
    /// 结合官方教程实现自动订阅消息转发
    /// </summary>
    public class WindsorMessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IServiceProvider service;

        /// <summary>
        /// 参数由中间件的AutoSubscriberMessageDispatcher进行传递
        /// </summary>
        /// <param name="service"></param>
        public WindsorMessageDispatcher(IServiceProvider service)
        {
            this.service = service;
        }

        /// <summary>
        /// 同步方法，可以解析TConsumer对应的服务
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <typeparam name="TConsumer"></typeparam>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        void IAutoSubscriberMessageDispatcher.Dispatch<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            //通过容器获取对应的消费者服务，也可以通过Autofac实现
            var consumer = service.GetService<IConsume<MessageDto>>();
            //通过解析到的服务调用对应的消费者实现的方法
            consumer.Consume(message as MessageDto, cancellationToken);
        }

        /// <summary>
        /// 异步方法
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <typeparam name="TConsumer"></typeparam>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task IAutoSubscriberMessageDispatcher.DispatchAsync<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken)
        {
            var consumer = service.GetService<TConsumer>();
            await consumer.ConsumeAsync(message, cancellationToken);
        }
    }
}
