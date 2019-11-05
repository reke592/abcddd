using System;
using hr.helper.errors;

namespace hr.domain.shared {
    /// shared value object
    public class Address : SharedKernel {
        public virtual Guid Id { get; protected set; }
        public virtual string LotBlock { get; protected set; }
        public virtual string Street { get; protected set; }
        public virtual string Municipality { get; protected set; }
        public virtual string Province { get; protected set; }
        public virtual string Country { get; protected set; }

        public static Address Create(
            string lotBlock, string street, string municipality,
            string province, string country) {
            
            using(var x = new ErrorBag()) {
                x.Required("street", street).Min(10).Max(30).AlphaNum();
                x.Required("municipality", municipality).Min(10).Max(30).Alpha();
                x.Required("province", province).Min(10).Max(30).Alpha();
                x.Required("country", country).Min(10).Max(30).Alpha();
            }

            return new Address {
                Id = Guid.NewGuid(),
                LotBlock = lotBlock,
                Street = street,
                Municipality = municipality,
                Province = province,
                Country = country
            };
        }

        public override bool valueEquals(object obj)
        {
            var other = obj as Address;
            return this.GetType() == other.GetType()
                && this.LotBlock.Equals(other.LotBlock)
                && this.Street.Equals(other.Street)
                && this.Municipality.Equals(other.Municipality)
                && this.Province.Equals(other.Province)
                && this.Country.Equals(other.Country);
        }

    }
}