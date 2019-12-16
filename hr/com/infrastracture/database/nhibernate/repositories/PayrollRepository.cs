using hr.com.domain.models.Payrolls;
using hr.com.domain.models.Payrolls.specs;

namespace hr.com.infrastracture.database.nhibernate.repositories {
    public class PayrollRepository : NHRepositoryBase<PayrollReport>, IPayrollRepository
    {
        public PayrollReport FindByPeriod(int month, int year)
        {
            return this.Find(new SpecificationPayrollReportOnSpecificPeriod(month, year));
        }
    }
}