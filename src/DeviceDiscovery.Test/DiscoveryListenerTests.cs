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
        public void T()
        {
            var fakeSocket = new MockedSocket();
            var fakeSocketFactory = A.Fake<ISocketFactory>();
            var fakeResponsFactory = A.Fake<IResponseFactory>();
            var messageParser = new MessageParser();
            var fakeDeviceInfoCollector = A.Fake<IDeviceInfoCollector>();

            A.CallTo(() => fakeSocketFactory.CreateListeningSocket()).Returns(fakeSocket);
            EndPoint dummyEndPoint = new IPEndPoint(IPAddress.Any, 0);

            var listener = new DiscoveryListener(fakeSocketFactory, fakeResponsFactory, messageParser, fakeDeviceInfoCollector);

            listener.Listen();
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

    internal class MockedSocket : ISocket
    {
        private const string messageString =
            "M-SEARCH * HTTP/1.1\r\n" +
            "LOCATION: http://1.1.1.1/service.xml\r\n" +
            "\r\n";

        public IAsyncResult BeginReceiveFrom(byte[] buffer, ref EndPoint remoteEndPoint, AsyncCallback asynCallback, object state)
        {
            var ar = new MockedAsyncResult(state);
            Encoding.ASCII.GetBytes(messageString).CopyTo(buffer, 0);

            asynCallback.Invoke(ar);

            return ar;
        }

        public int EndReceiveFrom(IAsyncResult ar, ref EndPoint remoteEndPoint)
        {
            return messageString.Length;
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