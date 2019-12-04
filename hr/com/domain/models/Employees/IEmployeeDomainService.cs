using hr.com.domain.shared;

namespace hr.com.domain.models.Employees {
    public interface IEmployeeDomainService {
        Employee RegisterNew(Person data);
    }
}