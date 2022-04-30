namespace EasyNetQ.WebApi
{
    [Queue("Qka.Order", ExchangeName = "Qka.Order")]
    public class MessageDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
