using RabbitMQ.Client;
using System.Text;

namespace dbclient.Connections
{
    public class UserTransferService
    {

        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public UserTransferService()
        {
          
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "rabbit_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void SendMessage(string msg)
        {
            var body = Encoding.UTF8.GetBytes(msg);

            _channel.BasicPublish(exchange: "", routingKey: "rabbit_queue", basicProperties: null, body: body);
        }


    }
}