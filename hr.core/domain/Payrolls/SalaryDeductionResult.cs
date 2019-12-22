namespace hr.core.domain.Payrolls {
    public enum SalaryDeductionResult {
        REQUESTED,          // deduction request created
        REQUEST_CANCELLED,  // cancelled by employee
        GRANTED,            // start payroll deduction
        RENEWED,            // new amoritzation with accumulated previous balance
        ADJUSTED,           // inform employee about the adjusted amount, grant on request
        NOT_GRANTED         // not granted by admin
    }
}