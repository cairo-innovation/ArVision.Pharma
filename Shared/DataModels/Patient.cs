using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Pharma.Shared.DataModels
{
    public class Patient : ICloneable
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public object Clone()
        {
            return new Patient
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName
            };
        }


        protected bool Equals(Patient other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Patient)obj);
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
