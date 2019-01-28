using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MedAvail.MedCenter.Common.Base.UdpMulticast.Data.Messages
{
    [Serializable]
    [ProtoContract(Name = @"ServiceDiscoveryResponse")]
    [DataContract(Name = @"ServiceDiscoveryResponse")]
    public class ServiceDiscoveryResponse
    {
        [ProtoMember(1, IsRequired = true, Name = @"Identifier", DataFormat = DataFormat.Default)]
        [DataMember(Name = @"Identifier", Order = 1, IsRequired = true)]
        public string Identifier { get; set; }

        [ProtoMember(2, IsRequired = true, Name = @"ServiceType", DataFormat = DataFormat.Default)]
        [DataMember(Name = @"ServiceType", Order = 2, IsRequired = true)]
        public string ServiceName { get; set; }

        [ProtoMember(3, IsRequired = true, Name = @"Version", DataFormat = DataFormat.Default)]
        [DataMember(Name = @"Version", Order = 3, IsRequired = true)]
        public string Version { get; set; }

        [ProtoMember(4, IsRequired = true, Name = @"Endpoint", DataFormat = DataFormat.Default)]
        [DataMember(Name = @"Endpoint", Order = 4, IsRequired = true)]
        public string Endpoint { get; set; }

        [ProtoMember(5, IsRequired = true, Name = @"Status", DataFormat = DataFormat.Default)]
        [DataMember(Name = @"Status", Order = 5, IsRequired = true)]
        public string Status { get; set; }
    }
}
