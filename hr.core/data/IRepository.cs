using System.Collections.Generic;

namespace hr.core.data
{
  public interface IRepository<T>
  {
    T find(int id);
    T find<K>(K id);
    IEnumerable<T> All();
    T save(T obj);
    T update(T obj);
    void delete(T obj);
  }
}