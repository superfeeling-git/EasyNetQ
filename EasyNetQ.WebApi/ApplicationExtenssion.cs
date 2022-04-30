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
        public static IApplicationBuilder UseSubscribe(this IApplicationBuilder appBuilder, string subscriptionIdPrefix, Assembly assembly)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;
            var lifeTime = services.GetService<IHostApplicationLifetime>();
            var bus = services.GetRequiredService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
                subscriber.AutoSubscriberMessageDispatcher = new WindsorMessageDispatcher(services);
                subscriber.Subscribe(new Assembly[] { assembly });
                subscriber.SubscribeAsync(new Assembly[] { assembly });
            }); 
            lifeTime.ApplicationStopped.Register(() => { bus.Dispose(); }); 
            return appBuilder;
        }
    }
}
