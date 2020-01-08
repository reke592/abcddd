using System;
using System.Collections.Generic;

namespace hr.core.domain.commons {
    public class Date : ValueObject {
        public virtual int Year { get; protected set; }
        public virtual int Month { get; protected set; }
        public virtual int Day { get; protected set; }

        public virtual string ShortMonth => Enum.GetName(typeof(ShortMonth), this.Month);
        public virtual string LongMonth => Enum.GetName(typeof(LongMonth), this.Month);

        public static Date Create(int year, int month, int day) {
            return TryParse(string.Join("/", year, month, day));
        }

        /// <summary>
        /// synonym Date.TryParse
        /// </summary>
        public static Date Create(string strDate) {
            return TryParse(strDate);
        }

        public static Date Now {
            get {
                return TryParse(DateTime.Now.ToLongDateString());
            }
        }

        public static Date TryParse(string strDate) {
            try {
                var d = DateTime.Parse(strDate);
                return new Date {
                    Year = d.Year,
                    Month = d.Month,
                    Day = d.Day
                };
            }
            catch {
                return null;
            }
        }

        public override string ToString() {
            return string.Format($"{Year}-{Month}-{Day}");
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Year;
            yield return Month;
            yield return Day;
        }
    }
}