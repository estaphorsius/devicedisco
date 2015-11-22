namespace DeviceDiscovery
{
    public class SearchRequestFactory : IRequestFactory
    {
        const string MessageHeader = "M-SEARCH * HTTP/1.1";
        const string MessageHost = "HOST: 239.255.255.250:1900";
        const string MessageMan = "MAN: \"ssdp:discover\"";
        const string MessageMx = "MX: 8";
        const string MessageSt = "ST: ssdp:all";


        public string CreateMessage()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{0}",
                "\r\n",
                MessageHeader,
                MessageHost,
                MessageMan,
                MessageMx,
                MessageSt);
        }
    }
}