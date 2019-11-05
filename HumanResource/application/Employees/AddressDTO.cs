namespace hr.application.Employees {
    public class AddressDTO {
        public virtual string LotBlock { get; protected set; }
        public virtual string Street { get; protected set; }
        public virtual string Municipality { get; protected set; }
        public virtual string Province { get; protected set; }
        public virtual string Country { get; protected set; }
    }
}