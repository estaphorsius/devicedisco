using NUnit.Framework;

namespace DeviceDiscovery.Test
{
    [TestFixture]
    public class MessageParserTests
    {
        [Test]
        public void VerifySearchMessageIsCorrectlyParsed()
        {
            //arrange
            MessageParser parser = new MessageParser();
            string rawString =
                "M-SEARCH HTTP/1.1\r\n" +
                "LOCATION: http://1.1.1.1/\r\n" +
                "\r\n";

            //act
            var message = parser.Parse(rawString);

            //assert
            Assert.AreEqual("M-SEARCH HTTP/1.1",message.MessageLine);
            Assert.AreEqual("http://1.1.1.1/", message.Headers["LOCATION"]);

        }
    }

}