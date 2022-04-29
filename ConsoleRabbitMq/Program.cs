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

            var queueName = nameof(Program.Main);

            //durable:队列的持久化，autoDelete必须为false
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

            //消息的持久化，否则RabbitMQ重启后只保留队列，不保留消息
            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            for (int i = 0; i < 50; i++)
            {
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: Encoding.UTF8.GetBytes(i + "---" +Guid.NewGuid().ToString()));
            }
        }
    }
}
