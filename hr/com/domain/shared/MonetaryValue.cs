using System;
using System.Collections.Generic;

// ulong max : 18,446,744,073,709,551,615

namespace hr.com.domain.shared {

    /// <summary>
    /// can support value upto ulong max - 10^precision.
    /// </summary>
    public class MonetaryValue {
        public const ushort DEFAULT_PRECISION = 6;

        // eg. PHP:6:100000
        public virtual string Raw { get; protected set; }
        public decimal PreciseValue { get; protected set; }
        public string Code { get; protected set; }

        public static MonetaryValue of(string code, decimal value) {
            return new MonetaryValue {
                Code = code,
                PreciseValue = value,
                Raw = $"{code}:{value}"
            };
        }

        public static MonetaryValue of(string raw, char splitter = ':') {
            try {
                var parts = raw.Split(splitter);
                if(parts.Length != 2)
                    throw new FormatException("Invalid MonetaryValue Format.");
                // may throw invalid format exception
                var value = decimal.Parse(parts[1]);

                return new MonetaryValue {
                    Code = parts[0],
                    PreciseValue = value,
                    Raw = raw
                };
            }
            catch (Exception e) {
                throw e;
            }
        }

        // where table is a fetched on database of updated currency table
        public MonetaryValue addValueOf(MonetaryValue other, IDictionary<string, decimal> table = null) {
            if((this.Code != other.Code)) {
                if(table is null)
                    throw new Exception("Can't add MonetaryValue, convertion table is null");
                var other_converted = other.PreciseValue * table[other.Code];
                return MonetaryValue.of(this.Code, this.PreciseValue + other_converted);
            }

            return MonetaryValue.of(this.Code, this.PreciseValue + other.PreciseValue);
        }

        public MonetaryValue subtractValueOf(MonetaryValue other, IDictionary<string, decimal> table = null) {
            if((this.Code != other.Code)) {
                if(table is null)
                    throw new Exception("Can't subtract MonetaryValue, convertion table is null");
                var other_converted = other.PreciseValue * table[other.Code];
                return MonetaryValue.of(this.Code, this.PreciseValue - other_converted);
            }
            
            return MonetaryValue.of(this.Code, this.PreciseValue - other.PreciseValue);
        }

        public MonetaryValue multipliedBy(decimal multiple) {
            return MonetaryValue.of(this.Code, this.PreciseValue * multiple);
        }

        public MonetaryValue dividedBy(decimal dividend) {
            return MonetaryValue.of(this.Code, this.PreciseValue / dividend);
        }

        /// <summary>
        /// multiply monetary value by per_unit.
        /// </summary>
        public static MonetaryValue Convert(MonetaryValue money, decimal per_unit, string code) {
            if(money is null) return null;
            return money.multipliedBy(per_unit);
        }

        public override string ToString() {
            return $"{this.Code}:{this.PreciseValue}";
        }
    }
}
