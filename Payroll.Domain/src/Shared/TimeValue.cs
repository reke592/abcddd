using System.Collections.Generic;

namespace Payroll.Domain.Shared {
    public class TimeValue : ValueObject {
        private int _hour;
        private int _minutes;

        public int Hour24 {
            get {
                return _hour % 24;
            }
        }

        public int Hour12{
            get {
                var value = _hour % 12;
                return (value > 1) ? value : 1;
            }
        }

        public int Minutes {
            get {
                return _minutes;
            }
        }

        public TimeSuffix Suffix {
            get {
                return (_hour < 12) ? TimeSuffix.AM : TimeSuffix.PM;
            }
        }

        public long As24HourMinutes {
            get {
                return (Hour24 * 60) + _minutes;
            }
        }

        /// <summary>
        /// 24 Hour format
        /// </summary>
        public static TimeValue of(int hour, int minutes) {
            return new TimeValue {
                _hour = hour,
                _minutes = minutes
            };
        }

        /// <summary>
        /// 12 Hour format, suffix: am, pm
        /// </summary>
        public static TimeValue of(int hour, int minutes, TimeSuffix suffix) {
            return new TimeValue {
                _hour = (suffix == TimeSuffix.AM) ? hour + 12 : hour,
                _minutes = minutes
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return As24HourMinutes;
        }
    }
}