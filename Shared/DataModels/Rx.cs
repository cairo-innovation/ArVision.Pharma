using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Pharma.Shared.DataModels
{
    public class Rx : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return new Rx
            {
                Id = Id,
                Name = Name
            };
        }


        protected bool Equals(Rx other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rx)obj);
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
