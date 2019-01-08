using System;

using System.Runtime.Serialization;
using Virtek.Common.Configuration.Data;
using Virtek.Common.Shared;

using ProtoBuf;

namespace Virtek.Base.UdpMulticast.Data.Messages
{
    [Serializable]
    [ProtoContract(Name = @"ServiceRunningMessage")]
    [DataContract(Name = @"ServiceRunningMessage")]
    public class ServiceRunningMessage
    {
        [ProtoMember(1, IsRequired = true, Name = @"RequestorIdentificationItem", DataFormat = DataFormat.Default)]
        [DataMember(Name = @"RequestorIdentificationItem", Order = 1, IsRequired = true)]
        public RequestorIdentificationItem RequestorIdentificationItem { get; set; }

        [ProtoMember(2, IsRequired = true, Name = @"RequestorServiceType", DataFormat = DataFormat.Default)]
        [DataMember(Name = @"RequestorServiceType", Order = 2, IsRequired = true)]
        public RequestorServiceType RequestorServiceType { get; set; }
    }
}
