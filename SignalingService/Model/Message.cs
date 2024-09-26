public class Message
{
    public long MessageId { get; set; }
    public string RoomId { get; set; }
    public string Sender { get; set; }
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
