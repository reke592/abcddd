using System;
using hr.helper.errors;

namespace hr.domain.shared {
    public enum LongMonth {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public enum ShortMonth {
        Jan = 1,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Aug,
        Sept,
        Oct,
        Nov,
        Dec
    }

    public class Date {
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
    }
}