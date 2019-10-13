namespace hr.core.bus
{
  public interface IDomainEventHandler<T, O>
    where T : IEvent<O>
    where O : class, IDTO
  {
    void HandleEvent(T _event);
  }
}
