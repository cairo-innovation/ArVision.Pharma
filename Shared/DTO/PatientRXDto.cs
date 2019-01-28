using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Service.Pharma.Shared.DTO
{
    public partial class PatientDto
    {
        public virtual IList<RXDto> RX { get; set; }
    }
}
