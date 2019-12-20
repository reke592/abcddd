namespace hr.com.domain.enums {
    public struct Unit {
        public const double WHOLE = 1;
        public const double HALF = 0.5;
        public const double QUARTER = 0.25;

        public static string Name(double unit) {
            switch(unit) {
                case WHOLE:
                    return "WHOLE";
                case HALF:
                    return "HALF";
                case QUARTER:
                    return "QUARTER";
                default:
                    return $"CUSTOM: {unit}";
            }
        }
    }
}