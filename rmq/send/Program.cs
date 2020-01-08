using System;
using RabbitMQ.Client;
using System.Text;

namespace send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
            using(var connection = factory.CreateConnection()) {
                using(var channel = connection.CreateModel()) {
                    channel.QueueDeclare("hello", 
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                    
                    string message = "Hello";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                        routingKey: "hello",
                                        basicProperties: null,
                                        body: body);
                    
                    Console.WriteLine($"Message sent: {message}");
                    Console.WriteLine("Press enter key to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
