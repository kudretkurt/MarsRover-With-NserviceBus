using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Shared
{
    public abstract class ValueObjectBase
    {
        /// <summary>
        /// When overriden in a derived class, returns all components of a value objects which constitute its identity.
        /// </summary>
        /// <returns>An ordered list of equality components.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if (object.ReferenceEquals(null, obj)) return false;
            if (this.GetType() != obj.GetType()) return false;
            var vo = obj as ValueObjectBase;
            return GetEqualityComponents().SequenceEqual(vo?.GetEqualityComponents() ?? throw new InvalidOperationException());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in GetEqualityComponents())
                    hash = hash * 23 + (obj != null ? obj.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
