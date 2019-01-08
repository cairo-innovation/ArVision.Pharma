using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Virtek.Core.Logging;

namespace Virtek.Base.UdpMulticast.Discovery
{
    public class DiscoveryClient : DiscoveryBase, IDisposable
    {
        private UdpClient udpClient;
        private IPEndPoint serverEp;
        private Thread discoveryThread;

        private readonly int port;
        private bool messageSent;

        private readonly object handleLock = new object();

        readonly Dictionary<string, ArrayList> handlersByType = new Dictionary<string, ArrayList>();

        public void SafeFireEvent(DiscoveryMessage discoveryMessage)
        {
            var obj = DeserializeObject(discoveryMessage.Message);

            if (handlersByType.ContainsKey(discoveryMessage.MessageKey))
            {
                ArrayList arrayList = null;


                lock (handleLock)
                {
                    arrayList = new ArrayList(handlersByType[discoveryMessage.MessageKey]);
                }

                foreach (var handler in arrayList)
                {
                    var action = arrayList[0];
                    var actionType = action.GetType();
                    actionType.GetMethod("Invoke").Invoke(action, new[] { obj });
                }
            }

            UIEvent?.Invoke(this, obj);
        }

        public DiscoveryClient(int port)
        {
            this.port = port;

            udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
            serverEp = new IPEndPoint(IPAddress.Any, 0);

        }
        public void Start()
        {
            discoveryThread = new Thread(ExecuteDiscoveryThread);
            discoveryThread.Name = "UDP Discovery Thread (Client)";
            discoveryThread.IsBackground = true;
            discoveryThread.Start();
        }

        private void ExecuteDiscoveryThread()
        {
            while (true)
            {
                if (messageSent == true)
                {
                    var serverResponseData = udpClient.Receive(ref serverEp);

                    try
                    {
                        var serverResponse = DeserializeObject<DiscoveryMessage>(serverResponseData);                        
                        SafeFireEvent(serverResponse);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
        
        public event EventHandler<object> UIEvent;

        public void RegisterHandler<T>(Action<T> action)
        {
            lock (handleLock)
            {
                if (handlersByType.ContainsKey(typeof(T).FullName))
                {
                    handlersByType[typeof(T).FullName].Add(action);
                }
                else
                {
                    handlersByType.Add(typeof(T).FullName, new ArrayList(new[] { action }));
                }
            }
        }

        public void UnregisterHandler<T>(Action<T> action)
        {
            lock (handleLock)
            {
                if (handlersByType.ContainsKey(typeof(T).FullName))
                {
                    handlersByType[typeof(T).FullName].Remove(action);
                }
            }
        }

        public void BroadcastMessage(object message)
        {
            LogManager.Logger.Trace($"{LogManager.GetCurrentMethodName(nameof(DiscoveryClient))}; messageType: ({message.GetType().FullName})");
            var discoveryMessage = SerializeObject(new DiscoveryMessage() { Message = SerializeObject(message), MessageKey = message.GetType().FullName });
            udpClient.Send(discoveryMessage, discoveryMessage.Length, new IPEndPoint(IPAddress.Broadcast, port));
            messageSent = true;
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
