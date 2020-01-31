using System;

namespace Payroll.Domain
{
  public class DomainAggregateGuid
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
    public static implicit operator DomainAggregateGuid(Guid id) => new DomainAggregateGuid(id);

    public override bool Equals(object obj)
    {
      var other = obj as DomainAggregateGuid;
      return this._value.ToString() == other._value.ToString();
    }

    public override int GetHashCode()
      => (this.GetType() + this._value.ToString()).GetHashCode();
    
    public static bool operator ==(DomainAggregateGuid a, DomainAggregateGuid b)
      => a.Equals(b);
    
    public static bool operator !=(DomainAggregateGuid a, DomainAggregateGuid b)
      => !a.Equals(b);

    public override string ToString()
      => _value.ToString();
  }
}