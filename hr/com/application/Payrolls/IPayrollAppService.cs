namespace hr.com.application.Payrolls {
    public interface IPayrollAppService {
        PayrollReportDTO GeneratePayroll(int month, int year, bool include_deductions = true);
    }
}