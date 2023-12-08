namespace Task7.API.Models
{
    public class Response
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public object? Content { get; set; }
        public int StatusCode { get; set; }
    }
}
