using System;
using hr.domain;
using hr.helper.database;

namespace hr.test {
    public class DummyRepositoryBase<T> : IRepository<T> where T : Entity
    {
        public void Delete(T obj)
        {
            Console.WriteLine($"delete: {obj.GetType().Name} Id: {obj.Id}");
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