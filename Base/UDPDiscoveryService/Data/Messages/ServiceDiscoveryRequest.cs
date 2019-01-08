using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Virtek.Base.UdpMulticast.Data.Messages
{
    [Serializable]
    [ProtoContract(Name = @"ServiceDiscoveryRequest")]
    [DataContract(Name = @"ServiceDiscoveryRequest")]
    public class ServiceDiscoveryRequest
    {

    }
}
