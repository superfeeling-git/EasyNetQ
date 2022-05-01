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
    /// <summary>
    /// IConsume接口的实现类
    /// </summary>
    public class OrderConsumer : IConsume<MessageDto>
    {
        private readonly ILogger<OrderConsumer> logger;

        /// <summary>
        /// 可以注入相关服务
        /// </summary>
        /// <param name="logger"></param>
        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 消息的消费方法
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        public void Consume(MessageDto message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation(JsonConvert.SerializeObject(message));
        }
    }
}
