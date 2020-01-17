namespace hris.xunit.units.application.Employees {
    public static class Contracts
    {
        public static class V1
        {
            public class UpdateBio
            {
                public string FirstName { get; set; }
                public string MiddleName { get; set; }
                public string LastName { get; set; }
                public string DateOfBirth { get; set; }
            }
        }
    }
}