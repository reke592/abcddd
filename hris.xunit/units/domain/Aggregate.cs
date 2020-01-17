using System;
using System.Linq;
using System.Collections.Generic;

namespace hris.xunit.units.domain {
    public abstract class Aggregate : Entity<Guid> {
        private IList<object> _changes = new List<object>();
        public long Version { get; private set; } = -1;

        protected abstract void When(object e);

        /// <summary>
        /// add changes to aggregate
        /// </summary>
        public void Apply(object e)
        {
            When(e);
            _changes.Add(e);
        }

        /// <summary>
        /// load aggregate from history
        /// </summary>
        public void Load(object[] history)
        {
            foreach(var e in history) {
                When(e);
                Version ++;
            }
        }

        public object[] Events => _changes.ToArray();
    }
}