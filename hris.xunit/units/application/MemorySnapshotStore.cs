using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using hris.xunit.units.EventSourcing;

namespace hris.xunit.units.application
{
    public class MemorySnapshotStore : ISnapshotStore
    {
        private IDictionary<Type, IDictionary<Guid, object>> _store 
            = new Dictionary<Type, IDictionary<Guid, object>>();

        private ISet<Guid> _dirty = new HashSet<Guid>();

        public void Store<T>(Guid id, T document)
        {
            var doc_type = typeof(T);
            if(_store.TryGetValue(doc_type, out var records))
            {
                records.Add(id, document);
            }
            else 
            {
                var new_collection = new Dictionary<Guid, object>();
                new_collection.Add(id, document);
                _store.Add(doc_type, new_collection);
            }
            
            _dirty.Add(id);
        }

        public void Delete<T>(Guid id)
        {            
            var doc_type = typeof(T);
            if(_store.TryGetValue(doc_type, out var records))
            {
                records.Remove(id);
                _dirty.Add(id);

                if(records.Count == 0)
                    _store.Remove(doc_type);
            }
        }

        public void UpdateIfFound<T>(Guid id, Action<T> apply)
        {
            var doc_type = typeof(T);
            if(_store.TryGetValue(doc_type, out var records)) {
                if(records.ContainsKey(id))
                {
                    Console.WriteLine($"apply: {typeof(T)}");
                    apply((T) records[id]);
                    _dirty.Add(id);
                }
            }
        }

        public Task SaveAsync()
        {
            if(_dirty.Count > 0)
                return new Task(() => Console.WriteLine($"push {_dirty.Count} projection updates to database"));
            
            return default(Task);
        }

        public T Get<T>(Guid id)
        {
            var doc_type = typeof(T);
            if(_store.TryGetValue(doc_type, out var documents))
            {
                if(documents.TryGetValue(id, out var record))
                {
                    return (T) record;
                }
            }
            return default(T);
        }

        public IReadOnlyCollection<T> All<T>()
        {
            if(_store.TryGetValue(typeof(T), out var documents))
            {
                return documents.Values.Cast<T>().ToList();
            }
            return default(ReadOnlyCollection<T>);
        }
    }
}