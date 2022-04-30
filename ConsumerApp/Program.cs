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

            channel.ExchangeDeclare($"Ex_{nameof(Program.Main)}", ExchangeType.Topic);

            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName, exchange: $"Ex_{nameof(Program.Main)}", routingKey: args[0]);

            var consumer = new EventingBasicConsumer(channel);

            //Qos设置在Received里无效
            //每次只接收一条消息，应答后队列下发另一条
            channel.BasicQos(0, 1, false);

            //接收
            consumer.Received += (sender, e) => {
                var tag = e.DeliveryTag;
                var key = e.RoutingKey;
                var exchange = e.Exchange;
                var body = e.Body.ToArray();
                Console.WriteLine(Encoding.UTF8.GetString(body));

                channel.BasicAck(e.DeliveryTag, false);
            };

            //消费
            channel.BasicConsume(queueName, false, consumer);

            Console.ReadLine();
        }
    }
}
