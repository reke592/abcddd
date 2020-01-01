using System.Collections.Generic;

namespace hr.core.domain.Employees {
    public class SalaryGrade : SharedEntity
    {
        public string Level { get; protected set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Level;
        }
    }
}