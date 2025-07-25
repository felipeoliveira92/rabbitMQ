using RabbitMQ.Client;
using SendMail.API.Messages;
using System.Text.Json;
using System.Text;

namespace SendMail.API.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly IConnection _connection;

        public RabbitMQMessageSender()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
        }

        public void SendMessage(EmailMessage message, string queueName)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
                var body = GetMessageAsByteArray(message);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }

        private byte[] GetMessageAsByteArray(EmailMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize<EmailMessage>(message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}
