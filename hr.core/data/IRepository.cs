using System.Collections.Generic;

namespace hr.core.data
{
  public interface IRepository<T> where T : Entity
  {
    T find(long id);
    IList<T> All();
    T save(T obj);
    T update(T obj);
    void delete(T obj);
  }
}