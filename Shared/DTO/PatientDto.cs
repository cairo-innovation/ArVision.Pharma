using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Service.Pharma.Shared.DTO
{
    public partial class PatientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PatientIMG { get; set; }
        public string PatientIdenficationIMG { get; set; }
        public int CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int DoctorId { get; set; }
    }
}
