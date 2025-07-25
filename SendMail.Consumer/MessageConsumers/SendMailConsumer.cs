using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SendMail.Consumer.Messages;
using System.Text;
using System.Text.Json;

namespace SendMail.Consumer.MessageConsumers
{
    public class SendMailConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public SendMailConsumer()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare("emailQueue", false, false, false, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                var message = JsonSerializer.Deserialize<EmailMessage>(content);

                // Ack manual, dizendo que a mensagem foi recebida e processada com sucesso
                _channel.BasicAck(evt.DeliveryTag, false);
            };

            //o parâmetro autoAck: true diz ao RabbitMQ para considerar a mensagem como processada e confirmada assim que ela é
            //entregue ao seu consumidor. A mensagem é removida da fila imediatamente
            _channel.BasicConsume("emailQueue", autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
