using log4net;

namespace DeviceDiscovery
{
    public class ResponseFactory : IResponseFactory
    {
        private readonly DeviceInformation _deviceInformation;
        private readonly IMessageParser _messageParser;
        private readonly ILog _log = LogManager.GetLogger(typeof(ResponseFactory));

        public ResponseFactory(DeviceInformation deviceInformation, IMessageParser messageParser)
        {
            _deviceInformation = deviceInformation;
            _messageParser = messageParser;
        }

        public string CreateResponse(string requestString)
        {
            var message = _messageParser.Parse(requestString);

            if (message.MessageLine.StartsWith(Constants.SearchMessage))
            {
                return CreateSearchResponse();
            }

            return null;
        }

        private string CreateSearchResponse()
        {
            var response =
                "HTTP/1.1 200 OK\r\n" +
                "LOCATION: " + _deviceInformation.Location + "\r\n" +
                "USN: " + _deviceInformation.UniqueId + "\r\n" +
                "SERVER: " + _deviceInformation.PlatformName + "\r\n" +
                "\r\n";

            return response;
        }
    }
}