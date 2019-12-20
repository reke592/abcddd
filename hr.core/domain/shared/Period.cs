namespace hr.core.domain.shared {
    // maybe we don't need this
    // use factory pattern instead
    public struct Period {
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