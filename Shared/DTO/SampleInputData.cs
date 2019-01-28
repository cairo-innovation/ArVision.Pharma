using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ArVision.Service.Pharma.Shared.DTO
{
    [Serializable]
    [DataContract]
    public class SampleInputData
    {
        public SampleInputData(int intergerData,string stringData)
        {
            IntergerData = intergerData;
            StringData = stringData;
        }

        public SampleInputData() : this(0,string.Empty)
        {
        }

        #region Properties
        [DisplayName("InegerData")]
        [DataMember(Order = 10)]
        public int IntergerData { get; set; }

        [DisplayName("StringData")]
        [DataMember(Order = 10)]
        public string StringData { get; set; }


        #endregion

        //public override string ToString()
        //{
        //    return $"API Version Number : ({VersionNumber})";
        //}
    }
}
