using System.Collections.Generic;

namespace hr.core.domain.Deductions {
    public class AccountName : ValueObject {
        private string _name;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _name;
        }
    }
}