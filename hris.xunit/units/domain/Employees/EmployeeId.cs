using System;

namespace hris.xunit.units.domain.Employees {
    public class EmployeeId {
        private readonly Guid _value;

        public EmployeeId(Guid id) {
            _value = id;
        }

        // we use the implicit operators to avoid typecasting
        // EmployeeId id = Guid.NewGuid();
        public static implicit operator EmployeeId(Guid value) => new EmployeeId(value);
        
        // Guid x = id;
        public static implicit operator Guid(EmployeeId self) => self._value;

        public override string ToString() => _value.ToString();
    }
}