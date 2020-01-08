using hr.core.domain.commons;

namespace hr.core.application.commons {
    public class TimeValueDTO {
        public int Hour { get; set; }
        public int Minutes { get; set; }
        public TimeSuffix Suffix { get; set; }
    }
}