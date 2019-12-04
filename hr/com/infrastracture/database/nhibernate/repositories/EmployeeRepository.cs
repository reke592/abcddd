using hr.com.domain.models.Employees;

namespace hr.com.infrastracture.database.nhibernate {
    public class EmployeeRepository : NHRepositoryBase<Employee>, IEmployeeRepository
    {
        public Employee findById(long id)
        {
            var session = NHibernateHelper.GetCurrentSession();
            return session.Get<Employee>(id);
        }
    }
}