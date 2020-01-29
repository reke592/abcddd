using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Payroll.EventSourcing;

namespace Payroll.Infrastructure
{
  public class MemoryCacheStore : ICacheStore
  {
    private IDictionary<Type, IDictionary<Guid, object>> _store 
      = new Dictionary<Type, IDictionary<Guid, object>>();

    public void Store<T>(Guid id, T document)
    {
      var doc_type = typeof(T);
      if(_store.TryGetValue(doc_type, out var records))
      {
        if(records.Keys.Contains(id))
        {
          // replace on re-projection
          records.Remove(id);
        }
        records.Add(id, document);
      }
      else 
      {
        var new_collection = new Dictionary<Guid, object>();
        new_collection.Add(id, document);
        _store.Add(doc_type, new_collection);
      }
    }

    public void Delete<T>(Guid id)
    {            
      var doc_type = typeof(T);
      if(_store.TryGetValue(doc_type, out var records))
      {
        records.Remove(id);

        if(records.Count == 0)
          _store.Remove(doc_type);
      }
    }

    public void UpdateIfFound<T>(Guid id, Action<T> apply)
    {
      var doc_type = typeof(T);
      if(_store.TryGetValue(doc_type, out var records))
      {
        if(records.ContainsKey(id))
        {
          apply((T) records[id]);
        }
      }
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

    public void DeleteAll<T>() {
      if(_store.TryGetValue(typeof(T), out var documents))
      {
        documents.Clear();
      }
    }

    public bool GetRecent<T>(out T record) {
      if(_store.TryGetValue(typeof(T), out var documents))
      {
        record = (T) documents.Values.Last();
        return true;
      }
      record = default(T);
      return false;
    }

    public bool GetRecent<T>(int n, out IEnumerable<T> records) {
      if(_store.TryGetValue(typeof(T), out var documents))
      {
        var skip = (n <= documents.Count) ? documents.Count - n : 0;
        records = documents.Values.Skip(skip).Cast<T>().ToList();
        return true;
      }
      records = null;
      return false;
    }
  }
}