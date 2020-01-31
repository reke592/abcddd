using System;

namespace Payroll.Domain
{
  public abstract class AggregateId<T>  where T : AggregateId<T>
  {
    public Guid Value { get; protected set; }

    public override bool Equals(object obj)
      => this.Value.ToString() == ((T) obj).Value.ToString();

    public override int GetHashCode()
      => (this.GetType() + this.Value.ToString()).GetHashCode();
    
    public static bool operator ==(AggregateId<T> a, AggregateId<T> b)
      => a.Equals(b);
    
    public static bool operator !=(AggregateId<T> a, AggregateId<T> b)
      => !a.Equals(b);

    public override string ToString()
      => this.Value.ToString();

    public static implicit operator AggregateId<T>(Guid value)
    {
      var instance = Activator.CreateInstance<T>();
      instance.Value = value;
      return instance;
    }

    public static implicit operator Guid(AggregateId<T> self) => self.Value;
  }
}