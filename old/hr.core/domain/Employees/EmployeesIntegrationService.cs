using System;
using hr.core.helper;
using hr.core.infrastracture;

namespace hr.core.domain.Employees {
    public class EmployeesIntegrationService : BaseHandler {
        [TargetEvent(typeof(events.EmployeeRecordCreated))]
        private void handle(object sender, events.EmployeeRecordCreated args) {
            Console.WriteLine("Employee created");
            Console.WriteLine(args.Employee);
        }

        [TargetEvent(typeof(IntegrationEvent))]
        private void handle(object sender, IntegrationEvent args) {
            Console.WriteLine(args);
        }

        [TargetEvent(typeof(events.errors.EmployeeNotCreated))]
        private void handle(object sender, events.errors.EmployeeNotCreated args) {
            Console.WriteLine(args.Message);
        }
    }
}