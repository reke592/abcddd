using System;
using Payroll.Domain.Users;

namespace Payroll.Domain.BusinessYears
{
  public static class Events
  {
    public static class V1
    {
      public class BusinessYearCreated
      {
        public BusinessYearId Id { get; set; }
        public int ApplicableYear { get; set; }
        public UserId CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
      }

      public class BusinessYearStarted
      {
        public BusinessYearId Id { get; set; }
        public int Year { get; set; }
        public UserId StartedBy { get; set; }
        public DateTimeOffset StartedAt { get; set; }
      }

      public class BusinessYearEnded
      {
        public BusinessYearId Id { get; set; }
        public int Year { get; set; }
        public UserId EndedBy { get; set; }
        public DateTimeOffset EndedAt { get; set; }
      }

      public class BusinessYearConsigneeCreated
      {
        public BusinessYearId Id { get; set;}
        public ConsigneePerson Consignee { get; set; }
        public UserId AddedBy { get; set; }
        public DateTimeOffset AddedAt { get; set; }
      }

      public class BusinessYearConsigneeUpdated
      {
        public BusinessYearId Id { get; set;}
        public ConsigneePerson OldValue { get; set; }
        public ConsigneePerson NewValue { get; set; }
        public UserId UpdatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
      }

      public class BusinessYearUpdateAttemptFailed
      {
        public BusinessYearId Id { get; set; }
        public string Reason { get; set; }
        public object AttemptedValue { get; set; }
        public UserId AttemptedBy { get; set; }
        public DateTimeOffset AttemptedAt { get; set; }
      }
    }
  }
}