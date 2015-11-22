using log4net;

namespace DemoApp
{
    public class ResponseFactory : IResponseFactory
    {
        private readonly DeviceInformation _deviceInformation;
        private readonly IMessageParser _messageParser;
        private readonly ILog _log = LogManager.GetLogger(typeof (ResponseFactory));

        public ResponseFactory(DeviceInformation deviceInformation, IMessageParser messageParser)
        {
            _deviceInformation = deviceInformation;
            _messageParser = messageParser;
        }

        public string CreateResponse(string requestString)
        {
            var message = _messageParser.Parse(requestString);

            if (message.MessageLine.StartsWith("M-SEARCH"))
            {
                return CreateSearchResponse(message);
            }
            else if (message.MessageLine.StartsWith("NOTIFY"))
            {
                // do not send a response but handle the notification 
                ProcessNotification(message);
            }

            return null;
        }

        private string CreateSearchResponse(Message requestMessage)
        {
            var response =
                "HTTP/1.1 200 OK\r\n" +
                "LOCATION: " + _deviceInformation.Location + "\r\n" +
                "USN: " + _deviceInformation.UniqueId + "\r\n" +
                "\r\n";

            return response;
        }

        private void ProcessNotification(Message notificationMessage)
        {
            _log.InfoFormat("NOTIFY FROM {0}", notificationMessage.Headers["LOCATION"]);
        }
    }
}