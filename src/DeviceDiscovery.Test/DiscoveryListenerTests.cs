using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using FakeItEasy;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DeviceDiscovery.Test
{
    [TestFixture]
    public class DiscoveryListenerTests
    {
        [Test]
        public void VerifyThatSearchRequestWillBeAnsweredCorrectly()
        {
            //arrange
            var fakeSocket = new MockedSearchSocket();
            var fakeSocketFactory = A.Fake<ISocketFactory>();
            var fakeResponsFactory = A.Fake<IResponseFactory>();
            var messageParser = new MessageParser();
            var fakeDeviceInfoCollector = A.Fake<IDeviceInfoCollector>();
            A.CallTo(() => fakeSocketFactory.CreateListeningSocket()).Returns(fakeSocket);
            EndPoint dummyEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Message receivedMessage = null;
            var listener = new DiscoveryListener(fakeSocketFactory, fakeResponsFactory, messageParser, fakeDeviceInfoCollector);
            listener.SearchMessageReceived += (sender, e) =>
            {
                receivedMessage = e;
            };

            //act
            listener.Listen();
            listener.Stop();
            //assert
            Assert.AreEqual("M-SEARCH * HTTP/1.1", receivedMessage.MessageLine);
            Assert.AreEqual("http://1.1.1.1/service.xml", receivedMessage.Headers["LOCATION"]);
        }

        [Test]
        public void VerifyThatNotifyMessageIsReceivedAndDeviceInformationIsPopulated()
        {
            //arrange
            var fakeSocket = new MockedNotifySocket();
            var fakeSocketFactory = A.Fake<ISocketFactory>();
            var fakeResponsFactory = A.Fake<IResponseFactory>();
            var messageParser = new MessageParser();
            var fakeDeviceInfoCollector = A.Fake<IDeviceInfoCollector>();

            A.CallTo(() => fakeSocketFactory.CreateListeningSocket()).Returns(fakeSocket);
            EndPoint dummyEndPoint = new IPEndPoint(IPAddress.Any, 0);
            DeviceInformation deviceInformation = null;
            A.CallTo(() => fakeDeviceInfoCollector.Collect(A<Message>.Ignored))
                .Returns(new DeviceInformation { Location = "http://1.1.1.1/service.xml" });
            var listener = new DiscoveryListener(fakeSocketFactory, fakeResponsFactory, messageParser, fakeDeviceInfoCollector);
            listener.DeviceDiscovered += (sender, e) =>
            {
                deviceInformation = e;
            };


            //act
            listener.Listen();

            //assert
            Assert.AreEqual("http://1.1.1.1/service.xml", deviceInformation.Location);
        }
    }

    internal class MockedAsyncResult : IAsyncResult
    {
        public bool IsCompleted { get; }
        public WaitHandle AsyncWaitHandle { get; }
        public object AsyncState { get; }
        public bool CompletedSynchronously { get; }

        public MockedAsyncResult(object state)
        {
            AsyncState = state;
        }
    }

    internal class MockedNotifySocket : ISocket
    {
        private const string MessageString =
            "NOTIFY * HTTP/1.1\r\n" +
            "LOCATION: http://1.1.1.1/service.xml\r\n" +
            "\r\n";

        public IAsyncResult BeginReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint, AsyncCallback asynCallback, object state)
        {
            var ar = new MockedAsyncResult(state);
            Encoding.ASCII.GetBytes(MessageString).CopyTo(buffer, 0);

            asynCallback.Invoke(ar);

            return ar;
        }

        public int EndReceiveFrom(IAsyncResult ar, ref EndPoint remoteEndPoint)
        {
            return MessageString.Length;
        }

        public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint)
        {
            throw new NotImplementedException();
        }

        public int SendTo(byte[] buffer, EndPoint remoteEndPoint)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }

    internal class MockedSearchSocket : ISocket
    {
        private const string MessageString =
            "M-SEARCH * HTTP/1.1\r\n" +
            "LOCATION: http://1.1.1.1/service.xml\r\n" +
            "\r\n";

        public IAsyncResult BeginReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint, AsyncCallback asynCallback, object state)
        {
            var ar = new MockedAsyncResult(state);
            Encoding.ASCII.GetBytes(MessageString).CopyTo(buffer, 0);

            asynCallback.Invoke(ar);

            return ar;
        }

        public int EndReceiveFrom(IAsyncResult ar, ref EndPoint remoteEndPoint)
        {
            return MessageString.Length;
        }

        public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint)
        {
            throw new NotImplementedException();
        }

        public int SendTo(byte[] buffer, EndPoint remoteEndPoint)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}