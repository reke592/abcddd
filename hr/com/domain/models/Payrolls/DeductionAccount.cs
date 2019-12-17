using System;

namespace hr.com.domain.models.Payrolls {
    public class DeductionAccount : ValueObject<Guid> {
        public virtual string Name { get; protected set; }
        
        public static DeductionAccount Create(string name) {
            var record = new DeductionAccount {
                Id = Guid.NewGuid()
                , Name = name
            };

            return record;
        }
    }
}