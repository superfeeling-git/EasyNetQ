using System;
using RabbitMQ.Client;
using System.Text;

namespace ConsoleRabbitMq
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare($"Ex_{nameof(Program.Main)}",ExchangeType.Direct);

            //Fanout/Direct/Topic模式需要消费者先启动
            //否则如果生产者先启动，会因为没有相应的队列绑定导致消息丢失
            do
            {
                Console.ReadLine();

                for (int i = 0; i < 50; i++)
                {
                    channel.BasicPublish(exchange: $"Ex_{nameof(Program.Main)}", routingKey: args[0], basicProperties: null, body: Encoding.UTF8.GetBytes(i + "---" + Guid.NewGuid().ToString()));
                }
            } while (true);
        }
    }
}
