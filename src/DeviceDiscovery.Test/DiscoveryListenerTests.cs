using System;
using System.Net;
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
            var fakeSocket = A.Fake<ISocket>();
            var fakeSocketFactory = A.Fake<ISocketFactory>();
            var fakeResponsFactory = A.Fake<IResponseFactory>();
            var fakeMessageParser = A.Fake<IMessageParser>();
            var fakeDeviceInfoCollector = A.Fake<IDeviceInfoCollector>();

            A.CallTo(() => fakeSocketFactory.CreateListeningSocket()).Returns(fakeSocket);
            EndPoint dummyEndPoint = new IPEndPoint(IPAddress.Any, 0);

            var listener = new DiscoveryListener(fakeSocketFactory, fakeResponsFactory, fakeMessageParser, fakeDeviceInfoCollector);
            A.CallTo(() => fakeSocket.EndReceiveFrom(A<IAsyncResult>.Ignored, ref dummyEndPoint)).
                Invokes(() => fakeSocket.EndReceiveFrom(A<IAsyncResult>.Ignored, ref dummyEndPoint));
            listener.Listen();
        }
    }
}