using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArVision.Pharma.Shared.DataModels
{
    public class Juices : ICloneable
    {
        public int Id { get; set; }
        public string JuiceName { get; set; }
        public object Clone()
        {
            return new Juices
            {
                Id = Id,
                JuiceName = JuiceName
            };
        }


        protected bool Equals(Juices other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Juices)obj);
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
