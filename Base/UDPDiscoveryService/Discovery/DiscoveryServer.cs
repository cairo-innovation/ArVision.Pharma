using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Virtek.Base.UdpMulticast.Data.Messages;

namespace Virtek.Base.UdpMulticast.Discovery
{
    public class DiscoveryServer : DiscoveryBase, IDisposable
    {
        private UdpClient udpClient;
        private Thread discoveryThread;
        private readonly int port;
        private readonly Dictionary<Type, object> responseMessageByRequestMessageType = new Dictionary<Type, object>();
        public event EventHandler<ServiceRunningMessage> ServerRequestsRegistration; 

        public DiscoveryServer(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            discoveryThread = new Thread(ExecuteDiscoveryThread);
            discoveryThread.Name = "UDP Discovery Thread (Server)";
            discoveryThread.IsBackground = true;
            discoveryThread.Start();
        }

        public void RegisterResponse(Type requestMessageType, object responseMessage)
        {
            if (responseMessageByRequestMessageType.ContainsKey(requestMessageType))
            {
                responseMessageByRequestMessageType[requestMessageType] = responseMessage;
            }
            else
            {
                responseMessageByRequestMessageType.Add(requestMessageType, responseMessage);
            }
        }


        private void ExecuteDiscoveryThread()
        {
            udpClient = new UdpClient(port);

            while (true)
            {
                IPEndPoint clientEp = new IPEndPoint(IPAddress.Any, 0);
                byte[] clientRequestData = udpClient.Receive(ref clientEp);

                try
                {
                    DiscoveryMessage discoveryMessage = DeserializeObject<DiscoveryMessage>(clientRequestData);
                    ServiceRunningMessage serviceRunningMessage = DeserializeObject<ServiceRunningMessage>(discoveryMessage.Message);

                    ServerRequestsRegistration?.Invoke(this, serviceRunningMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void Dispose()
        {
            if (discoveryThread != null)
            {
                discoveryThread.Abort();
                discoveryThread = null;
            }

            if (udpClient != null)
            {
                udpClient.Close();
                udpClient.Dispose();
                udpClient = null;
            }
        }
    }
}
