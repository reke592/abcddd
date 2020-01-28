using System;

namespace Payroll.Domain
{
  public abstract class DomainAggregateGuid
  {
    protected Guid _value;

    public DomainAggregateGuid()
    {
      _value = Guid.Empty;
    }

    public DomainAggregateGuid(Guid value)
    {
      if(value == Guid.Empty) throw new Exception("can't assign empty id to aggregate");
      _value = value;
    }

    public static implicit operator Guid(DomainAggregateGuid self) => self._value;

    public override string ToString() => _value.ToString();
  }
}