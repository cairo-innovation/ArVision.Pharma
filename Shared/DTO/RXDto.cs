using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Service.Pharma.Shared.DTO
{
    public class RXDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PatientId { get; set; }
        public int MedicineId { get; set; }
        public double Dose { get; set; }
        public int JuiceId { get; set; }
        public string IMG { get; set; }
        public string Notes { get; set; }
        public int SunDay { get; set; }
        public int MonDay { get; set; }
        public int TusDay { get; set; }
        public int WedDay { get; set; }
        public int ThrDay { get; set; }
        public int FriDay { get; set; }
        public int SatDay { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
