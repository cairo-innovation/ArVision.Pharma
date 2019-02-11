using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Service.Pharma.Shared.DTO
{
    public partial class VisitDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public int RXId { get; set; }
        public int CreatedUser { get; set; }
        public int UpdatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
