namespace hr.core.bus
{
  public interface IIntegrationEvent<T> : IEvent<T> where T : IDTO
  { }
}
