namespace hr.core.data
{
  public interface IDAO<T>
  {
    void save(T obj);
    void update(T obj);
    void delete(T obj);
  }
}