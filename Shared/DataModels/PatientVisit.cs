using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Pharma.Shared.DataModels
{
    public class PatientVisit : ICloneable
    {
             public int Id { get; set; }
            public Nullable<int> UserId { get; set; }
            public Nullable<int> PatientId { get; set; }
            public Nullable<int> DoctorId { get; set; }
            public Nullable<int> MedicinId { get; set; }
            public Nullable<int> JuiceId { get; set; }
            public Nullable<int> RxId { get; set; }
            public Nullable<int> DayWeek { get; set; }
            public Nullable<System.DateTime> VisitDate { get; set; }
            public Nullable<double> Dose { get; set; }
            public string Notes { get; set; }

            public virtual Doctor Doctor { get; set; }
            public virtual Juice Juice { get; set; }
            public virtual Medicin Medicin { get; set; }
            public virtual Patient Patient { get; set; }
            public virtual Rx Rx { get; set; }
            public virtual User User { get; set; }
        public object Clone()
        {
            return new PatientVisit
            {
                Id = Id
            };
        }


        protected bool Equals(PatientVisit other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PatientVisit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id * 397;
            }
        }

    }
}
