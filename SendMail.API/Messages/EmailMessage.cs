namespace SendMail.API.Messages
{
    public class EmailMessage
    {
        public long Id { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
    }
}
