using System;
using hr.core.domain.Employees.events.errors;
using hr.core.helper;
using hr.core.infrastracture;

namespace console.com {
    public class TestBaseHandler2 : BaseHandler
    {
        [TargetEvent(typeof(TestEventB))]
        public void handle(object sender, TestEventB args)
        {
            Console.WriteLine(args.SomeNumber * 2);
        }

        [TargetEvent(typeof(EmployeeNotCreated))]
        private void handle(object sender, EmployeeNotCreated args) {
            Console.WriteLine(args);
            Console.WriteLine(args.Message);
        }
    }
}