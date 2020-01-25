using Payroll.Domain.BusinessYears;

namespace Payroll.Application.BusinessYears
{
  public static class Contracts
  {
    public static class V1
    {
      public class CreateBusinessYear
      {
        public string AccessToken { get; set; }
        public int ApplicableYear { get; set; }
      }

      public class CreateConsignee
      {
        public string AccessToken { get; set; }
        public BusinessYearId BusinessYearId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
      }

      public class UpdateConsignee
      {
        public string AccessToken { get; set; }
        public BusinessYearId BusinessYearId { get; set; }
        public string OldName { get; set; }
        public string OldPosition { get; set; }
        public string NewName { get; set; }
        public string NewPosition { get; set; }
      }

      public class EndBusinessYear
      {
        public string AccessToken { get; set; }
        public BusinessYearId BusinessYearId { get; set; }
      }
    }
  }
}