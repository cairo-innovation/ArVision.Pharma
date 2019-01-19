using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArVision.Pharma.Shared.DataModels;
using ArVision.Service.Pharma.Shared.DTO;

namespace ArVision.Pharma.DataAccess
{
    public interface IPharmaRepository : IDisposable
    {
        List<Juices> GetJuiceList();

        List<LookUpDto> GetList(string table);

        List<Patient> GetPatientList();

        PatientDto AddPatient(PatientDto patient);
        RXDto AddRXToPatient(RXDto rx);
        PatientDto GetPatientWithRX(int id);
   }
}
