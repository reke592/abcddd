namespace hr.com.application.Payrolls {
    public interface IPayrollAppService {
        PayrollReportDTO GeneratePayroll(int month, int year, double month_unit, bool include_deductions = true);
    }
}