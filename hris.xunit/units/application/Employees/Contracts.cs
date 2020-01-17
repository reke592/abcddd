namespace hris.xunit.units.application.Employees {
    public static class Contracts
    {
        // ... how about, we use the component model annotations of .Net framework for validations?
        public static class V1
        {
            public class CreateEmployeeCommand
            {
                public string FirstName { get; set; }
                public string MiddleName { get; set; }
                public string LastName { get; set; }
                public string DateOfBirth { get; set; }
                public string CreatedAt { get; set; }
            }

            public class UpdateBioCommand
            {
                public string Id { get; set; }
                public string FirstName { get; set; }
                public string MiddleName { get; set; }
                public string LastName { get; set; }
                public string DateOfBirth { get; set; }
                public string UpdatedAt { get; set; }
            }

            public class ActivateEmployeeCommand
            {
                public string Id { get; set; }
                public string ActivatedAt { get; set; }
            }

            public class DeactivateEmployeeCommand
            {
                public string Id { get; set; }
                public string DeactivatedAt { get; set; }
            }
        }
    }
}