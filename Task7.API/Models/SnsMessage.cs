namespace Task7.API.Models
{
    public class SnsMessage
    {
        public string Type { get; set; }
        public string MessageId { get; set; }
        public string Token { get; set; }
        public string TopicArn { get; set; }
        public string Message { get; set; }
        public string SubscribeURL { get; set; }
        // Add other properties as needed

        // You may also need to define a class for the MessageAttributes property
        public Dictionary<string, SnsMessageAttribute> MessageAttributes { get; set; }
    }

    public class SnsMessageAttribute
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

}
