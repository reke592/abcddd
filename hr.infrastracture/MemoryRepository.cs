using System.Collections.Generic;
using System.Linq;
using hr.core.infrastracture;
using hr.core.domain;

namespace hr.infrastracture
{
    public class MemoryRepository<T> : IRepository<T> where T : Entity
    {
        private long _current_id;
        IList<T> _db = new List<T>();

        public long NextId {
            get {
                return ++_current_id;
            }
        }

        public T Find(Specification<T> spec)
        {
            return _db.AsQueryable().FirstOrDefault(spec.toExpression());
        }

        public IList<T> FindAll(Specification<T> spec)
        {
            return _db.AsQueryable().Where(spec.isSatisfiedBy).ToList();
        }

        public void Add(T obj)
        {
            if(_db.Contains(obj)) {
                _db.Remove(obj);
            }
            _db.Add(obj);
        }

        public bool Remove(T obj)
        {
            return _db.Remove(obj);
        }

        public T Save(T obj)
        {
            throw new System.NotImplementedException();
        }

        public void Update(T obj)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
