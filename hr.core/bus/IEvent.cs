namespace hr.core.bus
{
  public interface IEvent<T>
    where T : IDTO
  {
    string Name { get; }
    T Data { get; }
  }
}