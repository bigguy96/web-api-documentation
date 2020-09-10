namespace ConsoleApp.Entities
{
    public class RequestBody
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string ContentType { get; internal set; }
    }
}