using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Payroll.Domain;
using Payroll.EventSourcing;
using Payroll.EventSourcing.Serialization;

namespace Payroll.Infrastructure
{
  public class MemoryEventStore : IEventStore, IYAMLSerializable
  {
    private event EventHandler<object[]> _afterSave;
    private event EventHandler<object[]> _afterDBReload;
    private ITypeMapper _mapper;
    private IYAMLSerializer _serializer;
    public object[] Empty => new object[] { };
    IDictionary<Guid, IList<Event>> _store = new Dictionary<Guid, IList<Event>>();

    public MemoryEventStore(ITypeMapper mapper)
    {
        _mapper = mapper;
    }

    public MemoryEventStore(ITypeMapper mapper, IYAMLSerializer serializer)
    {
      _mapper = mapper;
      _serializer = serializer;
    }

    public object[] Get<T>(Guid id) where T : Aggregate
    {
      if(_store.ContainsKey(id))
      {
        return _store[id].Select(x => x.Metadata).ToArray();
      }
      return Empty;
    }

    public void Save<T>(T record) where T : Aggregate
    {
      if(record.Version != LatestVersion<T>(record))
        throw new DataIntegrityException(record);
      else
      {
        if(!_store.ContainsKey(record.Id)) {
          _store.Add(record.Id, new List<Event>());
        }

        foreach(var e in record.Events)
          _store[record.Id].Add(new Event(_mapper.GetEventName(e), e));
        
        // inform projection manager to update projections
        _afterSave?.Invoke("store.afterSave", record.Events);
      }
    }

    public long LatestVersion<T>(T record) where T : Aggregate
    {
      if(_store.ContainsKey(record.Id))
        return _store[record.Id].Count - 1;
      return -1;
    }

    public object[] GetPreviousVersion<T>(Guid id, int versionOffset = 1)
    {
      if(_store.TryGetValue(id, out var stream)) {
        versionOffset = (versionOffset >= stream.Count) ? stream.Count - 1 : versionOffset;
        return stream.SkipLast(versionOffset).Select(x => x.Metadata).ToArray();
      }
      return Empty;
    }

    public void AfterSave(EventHandler<object[]> handler)
    {
      _afterSave += handler;
    }

    public void AfterDBReload(EventHandler<object[]> handler) {
      _afterDBReload += handler;
    }

    public void YAML_Load(string raw)
    {
      var stream = _serializer.Deserialize<IDictionary<Guid, IList<Event>>>(raw);
      foreach(var events in stream.Values)
      {
        _afterDBReload?.Invoke("store.afterDbReload", events.Select(x => x.Metadata).ToArray());
      }
      _store = stream;
    }

    public string YAML_Export()
    {
      return _serializer.Serialize(new ReadOnlyDictionary<Guid, IList<Event>>(_store));
    }

    public bool TryGet<T>(Guid id, out object[] events) {
      throw new NotImplementedException();
    }
  }
}