namespace hr.application.Employees {
    public class AddressDTO {
        public virtual string LotBlock { get; set; }
        public virtual string Street { get; set; }
        public virtual string Municipality { get; set; }
        public virtual string Province { get; set; }
        public virtual string Country { get; set; }
    }
}