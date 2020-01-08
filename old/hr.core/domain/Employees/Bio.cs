using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hr.core.domain.Employees.rules;
using hr.core.domain.commons;
using hr.core.helper;

namespace hr.core.domain.Employees {
    public class Bio : Entity {
        private long _employee_id;
        private long _home_address_id;
        private long _present_address_id;

        public virtual string FirstName { get; protected set; }
        public virtual string MiddleName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string ExtName { get; protected set; }
        public virtual Gender Gender { get; protected set; }
        public virtual Date Birthdate { get; protected set; }
        public virtual Address PresentAddress { get; protected set; }
        public virtual Address HomeAddress { get; protected set; }
        
        
        [TargetCommand(typeof(commands.SetHomeAddress))]
        private void handle(object sender, commands.SetHomeAddress args) {
            if(args.Employee.Id == _employee_id) {
                _home_address_id = args.Address.Id;
                Broker.Emit(new events.EmployeeBioUpdated(args.Employee, this));
            }
        }

        [TargetCommand(typeof(commands.SetPresentAddress))]
        private void handle(object sender, commands.SetPresentAddress args) {
            if(args.Employee.Id == _employee_id) {
                _present_address_id = args.Address.Id;
                Broker.Emit(new events.EmployeeBioUpdated(args.Employee, this));
            }
        }

        public static Bio Create(
            string firstname, string middlename, string lastname
            , string extname, Gender gender, Date birthdate) {

            var record = new Bio {
                // Id = Guid.NewGuid(),
                FirstName = firstname,
                MiddleName = middlename,
                LastName = lastname,
                ExtName = extname,
                Gender = gender,
                Birthdate = birthdate
            };

            return record;
        }

    }
}