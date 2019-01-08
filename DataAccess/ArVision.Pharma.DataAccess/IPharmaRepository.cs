using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArVision.Pharma.Shared.DataModels;

namespace ArVision.Pharma.DataAccess
{
    public interface IPharmaRepository : IDisposable
    {
        List<Juice> GetJuiceList();

        List<Patient> GetPatientList();

    }
}
