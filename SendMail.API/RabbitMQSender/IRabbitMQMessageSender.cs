using SendMail.API.Messages;

namespace SendMail.API.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(EmailMessage message, string queueName);
    }
}
