using System;
using Payroll.Domain;

namespace Payroll.EventSourcing
{
    public class DataIntegrityException : Exception
    {
        public object[] FailedUpdates { get; }
        public DataIntegrityException(Aggregate record) 
            : base($"Can't apply updates to {record.GetType()}. Record version did not match.")
        {
            FailedUpdates = record.Events;
        }
    }
}