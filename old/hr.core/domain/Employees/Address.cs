using System.Collections.Generic;
using hr.core.domain.Employees.rules;
using hr.core.helper;

namespace hr.core.domain.Employees {
    public class Address : SharedEntity {
        public string LotBlock { get; protected set; }
        public string Street { get; protected set; }
        public string Municipality { get; protected set; }
        public string Province { get; protected set; }

        // private static readonly ValidAddressRule _validity_rule = new ValidAddressRule();
        // public bool IsValid => _validity_rule.isSatisfiedBy(this);

        public static Address Create(string lotblock, string street, string municipality, string province) {
            var record = new Address {
                LotBlock = lotblock,
                Street = street,
                Municipality = municipality,
                Province = province
            };

            return record;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return LotBlock;
            yield return Street;
            yield return Municipality;
            yield return Province;
        }
    }
}