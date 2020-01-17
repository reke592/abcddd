using System;
using RabbitMQ.Client;
using RabbitMQ.Util;
using System.Text;
using System.Diagnostics;

namespace send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "192.168.99.100" };
            using(var connection = factory.CreateConnection()) {
                int i = 0;
                var sw = new Stopwatch();
                sw.Start();
                while(i++ < 10)
                    using(var channel = connection.CreateModel()) {
                        channel.QueueDeclare("hello", 
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);
                        
                        string message = "Hello";
                        var body = Encoding.UTF8.GetBytes(message);
                        var prop = channel.CreateBasicProperties();
                        prop.CorrelationId = Guid.NewGuid().ToString();
                        channel.BasicPublish(exchange: "",
                                            routingKey: "hello",
                                            basicProperties: prop,
                                            body: body);
                        
                        // Console.WriteLine($"Message sent: {message}");
                        // Console.WriteLine("Press enter key to exit.");
                        // Console.ReadLine();
                    }
                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);
            }
        }
    }
}
