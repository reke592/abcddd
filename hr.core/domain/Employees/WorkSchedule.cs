using System.Collections.Generic;
using hr.core.domain.shared;

namespace hr.core.domain.Employees {
    public class WorkSchedule : SharedEntity
    {
        public TimeValue Start { get; protected set; }
        public TimeValue End { get; protected set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Start.As24HourMinutes;
            yield return End.As24HourMinutes;
        }
    }
}