using System;
using System.Collections.Generic;
using hr.com.domain;
using hr.com.helper.database;

namespace hr.xunit.units {
    public class DummyRepository<T> : IRepository<T> where T : Entity
    {
       public void Delete(T obj)
        {
            Console.WriteLine($"delete: {obj.GetType().Name} Id: {obj.Id}");
        }

        public T Find(Specification<T> spec)
        {
            throw new NotImplementedException();
        }

        public IList<T> FindAll(Specification<T> spec)
        {
            throw new NotImplementedException();
        }

        public T Save(T obj)
        {
            Console.WriteLine($"save: {obj.GetType().Name} Id: {obj.Id}");
            return obj;
        }

        public void SaveOrUpdate(T obj)
        {
            Console.WriteLine($"save or update: {obj.GetType().Name} Id: {obj.Id}");
        }

        public void Update(T obj)
        {
            Console.WriteLine($"update: {obj.GetType().Name} Id: {obj.Id}");
        }
    }
}