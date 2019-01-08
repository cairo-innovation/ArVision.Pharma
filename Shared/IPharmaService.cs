using System;
using System.Collections.Generic;
using ArVision.Pharma.Shared.DataModels;
using ArVision.Service.Pharma.Shared.DTO;

namespace ArVision.Service.Pharma.Shared
{
    public interface IPharmaService : IDisposable
    {

        /// <summary>
        /// Returns the version of the service
        /// </summary>
        /// <returns></returns>
        ApiVersion GetVersion();
        void Test(SampleInputData sampleInputData);
        List<Juice> GetJuiceList();
        List<Patient> GetPatientList();
    }
}
