using hr.com.helper.database;

namespace hr.com.domain.models.Payrolls {
    public interface IPayrollRepository : IRepository<PayrollReport> {
        PayrollReport FindByPeriod(int month, int year);
    }
}