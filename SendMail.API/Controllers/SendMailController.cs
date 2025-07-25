using Microsoft.AspNetCore.Mvc;
using SendMail.API.Messages;
using SendMail.API.RabbitMQSender;

namespace SendMail.API.Controllers
{
    [Route("api/[controller]")]
    public class SendMailController : Controller
    {
        [HttpPost]
        public IActionResult SendAsync(string to, string body)
        {
            var message = new EmailMessage
            {
                Id = 1,
                To = to,
                Body = body
            };

            new RabbitMQMessageSender().SendMessage(message, "emailQueue");
            return Json(new
            {
                success = true,
                message = "Email sent successfully",
            });
        }
    }
}
