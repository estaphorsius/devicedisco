using System;
using System.Collections.Generic;
using System.IO;

namespace DeviceDiscovery
{
    public class MessageParser : IMessageParser
    {
        public Message Parse(string rawMessage)
        {
            Message message = new Message();
            using (var reader = new StringReader(rawMessage))
            {
                var line = reader.ReadLine();
                var lineIdx = 0;
                while (line != null)
                {
                    if (lineIdx == 0)
                    {
                        // request line %METHOD% %URI% HTTP/1.1\r\n
                        message.MessageLine = line;
                    }
                    else
                    {
                        // headers %KEY%:%VALUE%
                        var header = ParseHeader(line);
                        if (header.Key != string.Empty)
                        {
                            message.Headers.Add(header.Key.ToUpper(), header.Value);
                        }
                    }

                    lineIdx++;
                    line = reader.ReadLine();
                }
            }

            return message;
        }

        private static KeyValuePair<string, string> ParseHeader(string headerString)
        {
            var idx = headerString.IndexOf(":", StringComparison.Ordinal);
            if (idx < 0)
            {
                return new KeyValuePair<string, string>("", "");
            }

            return new KeyValuePair<string, string>(
                headerString.Substring(0, idx).Trim(),
                headerString.Substring(idx + 1).Trim());

        }
    }
}