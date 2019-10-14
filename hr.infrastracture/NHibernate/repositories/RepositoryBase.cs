using System.Collections.Generic;
using hr.core.data;

namespace hr.infrastracture.NHibernate.repositories
{
  public abstract class RepositoryBase<T> : IRepository<T> where T : Entity
  {
    public IList<T> All() {
      using (var s = NHibernateHelper.OpenSession()) {
        return s.CreateCriteria<T>().List<T>();
      }
    }

    public void delete(T obj) {
      using (var s = NHibernateHelper.OpenSession()) {
        s.Delete(obj);
      }
    }

    public T find(long id) {
      using (var s = NHibernateHelper.OpenSession()) {
        return s.Get<T>(id);
      }
    }

    public T save(T obj) {
      using (var s = NHibernateHelper.OpenSession()) {
        s.Save(obj);
        return obj;
      }
    }

    public T update(T obj) {
      using (var s = NHibernateHelper.OpenSession()) {
        s.Update(obj);
        return obj;
      }
    }
  }
}