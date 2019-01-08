using System;

namespace Virtek.Base.UdpMulticast.Discovery
{
    [Serializable]
    public class DiscoveryMessage
    {
        public string MessageKey { get; set; }
        public byte[] Message { get; set; }
    }
}
