using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace EasyNetQ.WebApi
{
    public static class ApplicationExtenssion
    {
        /// <summary>
        /// 中间件实现
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <param name="subscriptionIdPrefix"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSubscribe(this IApplicationBuilder appBuilder, string subscriptionIdPrefix, Assembly assembly)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;
            var lifeTime = services.GetService<IHostApplicationLifetime>();
            var bus = services.GetRequiredService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
                //需要指定AutoSubscriberMessageDispatcher对应的实例
                //并可以通过构造函数传参，如:IServicesProvider，即:services
                subscriber.AutoSubscriberMessageDispatcher = new WindsorMessageDispatcher(services);
                subscriber.Subscribe(new Assembly[] { assembly });
                subscriber.SubscribeAsync(new Assembly[] { assembly });
            }); 
            lifeTime.ApplicationStopped.Register(() => { bus.Dispose(); }); 
            return appBuilder;
        }
    }
}
