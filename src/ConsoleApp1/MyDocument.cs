using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class MyDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public int MyProperty { get; set; }
    }
}
