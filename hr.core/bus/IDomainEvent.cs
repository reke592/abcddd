namespace hr.core.bus
{
  public interface IDomainEvent<T> : IEvent<T> where T : IDTO
  { }
}