using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "192.168.99.100" };
            using(var connection = factory.CreateConnection()) {
                using(var channel = connection.CreateModel()) {
                    channel.QueueDeclare("hello", 
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                    
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) => {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"---\n{message}");
                        Console.WriteLine(ea.DeliveryTag);
                        Console.WriteLine(ea.ConsumerTag);
                        Console.WriteLine(ea.BasicProperties.CorrelationId);
                        // channel.BasicAck(ea.DeliveryTag, false);
                    };

                    channel.BasicConsume(queue: "hello",
                                        autoAck: true,
                                        consumer: consumer);
                    
                    Console.WriteLine("Press enter key to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
