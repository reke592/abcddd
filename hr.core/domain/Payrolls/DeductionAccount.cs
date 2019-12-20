namespace hr.core.domain.Payrolls {
    public class DeductionAccount : Entity {
        public virtual string Name { get; protected set; }
        
        public static DeductionAccount Create(string name) {
            var record = new DeductionAccount {
                Name = name
            };

            return record;
        }
    }
}