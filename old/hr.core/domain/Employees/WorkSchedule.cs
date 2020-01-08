using System.Collections.Generic;
using hr.core.domain.commons;

namespace hr.core.domain.Employees {
    public class WorkSchedule : Entity
    {
        public TimeValue Start { get; protected set; }
        public TimeValue End { get; protected set; }
    }
}