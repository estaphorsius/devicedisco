using System.Collections.Generic;

namespace DemoApp
{
    public class Message
    {
        public Message()
        {
            Headers = new Dictionary<string, string>();
        }
        public string MessageLine { get; set; }
        public Dictionary<string, string> Headers { get; private set; }
    }
}