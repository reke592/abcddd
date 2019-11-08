using hr.domain.models.Employees;

namespace hr.infrastracture.database.nhibernate {
    public class EmployeeRepository : NHRepositoryBase<Employee>, IEmployeeRepository
    {
        public Employee findById(long id)
        {
            var session = NHibernateHelper.GetCurrentSession();
            return session.Get<Employee>(id);
        }
    }
}