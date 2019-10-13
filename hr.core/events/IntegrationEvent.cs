using hr.core.bus;

namespace hr.core.events
{
  public class IntegrationEvent<T> : IEvent<T> where T : IDTO
  {
    private string _name;
    private T _data;

    public IntegrationEvent(string name, T data) {
      this._name = name;
      this._data = data;
    }

    public string Name => _name;

    public T Data => _data;
  }
}