
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ArVision.Service.Pharma.Shared.DTO
{
    [Serializable]
    [DataContract]
    public class ApiVersion
    {
        public ApiVersion(string versionNumber)
        {
            VersionNumber = versionNumber;
        }

        public ApiVersion() : this(string.Empty)
        {
        }

        #region Properties
        [DisplayName("Version Number")]
        [DataMember(Order = 10)]
        public string VersionNumber { get; private set; }
        #endregion

        public override string ToString()
        {
            return $"API Version Number : ({VersionNumber})";
        }
    }
}
