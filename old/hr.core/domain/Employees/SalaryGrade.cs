using System.Collections.Generic;
using hr.core.domain.commons;

namespace hr.core.domain.Employees {
    public class SalaryGrade : Entity
    {
        public string Level { get; protected set; }
        public MonetaryValue Gross { get; protected set; }
    }
}