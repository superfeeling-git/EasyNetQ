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

            channel.ExchangeDeclare($"Ex_{nameof(Program.Main)}",ExchangeType.Fanout);

            for (int i = 0; i < 50; i++)
            {
                channel.BasicPublish(exchange: $"Ex_{nameof(Program.Main)}", routingKey: "", basicProperties: null, body: Encoding.UTF8.GetBytes(i + "---" +Guid.NewGuid().ToString()));
            }
        }
    }
}
