using System.Collections.Generic;

namespace hr.core.domain.Employees {
    public class Address : SharedEntity {
        public string LotBlock { get; protected set; }
        public string Street { get; protected set; }
        public string Municipality { get; protected set; }
        public string Province { get; protected set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return LotBlock;
            yield return Street;
            yield return Municipality;
            yield return Province;
        }
    }
}