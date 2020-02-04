using System;
using System.Collections.Generic;

namespace Payroll.Domain.Shared {
    public class Date : ValueObject {
        public virtual int Year { get; protected set; }
        public virtual int Month { get; protected set; }
        public virtual int Day { get; protected set; }

        public virtual string ShortMonth => Enum.GetName(typeof(ShortMonth), this.Month);
        public virtual string LongMonth => Enum.GetName(typeof(LongMonth), this.Month);

        public Date AddDays(int days)
        {
            var adjusted = DateTime.Parse(this.ToString());
            adjusted = adjusted.AddDays(days);
            this.Year = adjusted.Year;
            this.Day = adjusted.Day;
            this.Month = adjusted.Month;
            return this;
        }

        public Date AddMonth(int months)
        {
            var adjusted = DateTime.Parse(this.ToString());
            adjusted = adjusted.AddMonths(months);
            this.Year = adjusted.Year;
            this.Day = adjusted.Day;
            this.Month = adjusted.Month;
            return this;
        }

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

        public static Date TryParse(string strDate, bool @throw = false) {
            try {
                var d = DateTime.Parse(strDate);
                return new Date {
                    Year = d.Year,
                    Month = d.Month,
                    Day = d.Day
                };
            }
            catch {
                if(@throw)
                    throw new FormatException($"Invalid date format {strDate}");
                else
                    return null;
            }
        }

        /// <summary>
        /// compare b to a, 0 if equal, 1 if greater, -1 if less than
        /// </summary>
        public static int Compare(Date a, Date b)
        {
            // current
            if(a.Equals(b)) return 0;
            // past
            if(b.Year == a.Year && b.Month == a.Month && b.Day < a.Day) return -1;
            if(b.Year == a.Year && b.Month < a.Month) return -1;
            if(b.Year < a.Year) return -1;
            // future
            return 1;
        }

        public static int ComapreNow(Date other)
        {
            return Date.Compare(Date.Now, other);
        }

        public bool isPast()
        {
            return Date.ComapreNow(this) == -1;
        }

        public bool isFuture()
        {
            return Date.ComapreNow(this) == 1;
        }

        public bool isToday()
        {
            return Date.ComapreNow(this) == 0;
        }

        public override string ToString() {
            return string.Format($"{Year}/{Month}/{Day}");
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Year;
            yield return Month;
            yield return Day;
        }
    }
}