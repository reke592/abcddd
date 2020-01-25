using System;

namespace Payroll.Domain.Users
{
  public static class Events 
  {
    public static class V1
    {
      public class UserCreated
      {
        public UserId Id { get; set; }
        public string Username { get; set; }
        public string PassHash { get; set; }
        public UserId CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
      }

      public class UserPasswordChanged
      {
        public UserId Id { get; set; }
        public string NewPassHash { get; set; }
        public UserId ChangedBy { get; set; }
        public DateTimeOffset ChangedAt { get; set; }
      }

      public class UserUpdateAttemptFailed
      {
        public UserId Id { get; set; }
        public string Reason { get; set; }
        public object AttemptedValue { get; set; }
        public UserId AttemptedBy { get; set; }
        public DateTimeOffset AttemptedAt { get; set; }
      }
    }
  }
}