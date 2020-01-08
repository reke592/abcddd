using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using hr.core.helper;
using AutoMapper;

namespace hr.core.infrastracture {
    // message broker to use rabbitMQ
    public class RMQBroker : BaseHandler, IMessageBroker<BasicDeliverEventArgs>
    {
        [ThreadStatic]
        private static ConnectionFactory _factory;

        [ThreadStatic]
        private static IConnection _connection;

        private static IMapper _mapper;

        private static ISerializingStrategy _serializer;

        /// <summary>
        /// A service to forward all Integration event to RabbitMQ.
        /// </summary>
        public RMQBroker(IMapper mapper, ISerializingStrategy serializer, string hostname = "localhost", int port = 5672) {
            _factory = new ConnectionFactory { HostName = hostname , Port = port }; 
            _connection = _factory.CreateConnection();
            _mapper = mapper;
            _serializer = serializer;
        }

        ~RMQBroker() {
            _connection.Close();
            _connection.Dispose();
            _serializer = null;
            _mapper = null;
            _factory = null;
        }

        public void onReceive(object sender, BasicDeliverEventArgs args)
        {
            throw new NotImplementedException();
        }

        [TargetEvent(typeof(IntegrationEvent))]
        public void send(object sender, IntegrationEvent args)
        {           
            using(var channel = _connection.CreateModel()) {
                var obj = new {
                    Event = args.Integration,
                    Entity = args.EntityType.Name,
                    Data = _mapper.Map(args.Data, args.EntityType, args.TypeDTO, opts => {})
                };
                var message = _serializer.Serialize(obj);
                var body = Encoding.UTF8.GetBytes(message);

                channel.QueueDeclare("hello", 
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);                

                channel.BasicPublish(exchange: "",
                                    routingKey: "hello",
                                    basicProperties: null,
                                    body: body);
            }
        }
    }
}