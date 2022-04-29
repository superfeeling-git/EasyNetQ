using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            var queueName = nameof(Program.Main);

            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);

            //接收
            consumer.Received += (sender, args) => {
                var tag = args.DeliveryTag;
                var key = args.RoutingKey;
                var exchange = args.Exchange;
                var body = args.Body.ToArray();
                Console.WriteLine(Encoding.UTF8.GetString(body));
                channel.BasicAck(args.DeliveryTag, false);
            };

            //消费
            channel.BasicConsume(queueName, false, consumer);

            Console.ReadLine();
        }
    }
}
