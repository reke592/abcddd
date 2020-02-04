using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Payroll.Domain;
using Payroll.EventSourcing;
using Payroll.EventSourcing.Serialization.YAML;

namespace Payroll.Infrastructure
{
  public class MemoryEventStore : IEventStore, IYAMLSerializable
  {
    private event EventHandler<object[]> _afterSave;
    private event EventHandler<object[]> _afterDBReload;
    private ITypeMapper _mapper;
    private IYAMLSerializer _serializer;
    public object[] Empty => new object[] { };
    // IDictionary<Guid, IList<Event>> _store = new Dictionary<Guid, IList<Event>>();

    // TODO:
    // having multiple aggregate streams
    // create an anti-corruption layer so we can determine which stream to load first on db_reload
    private long EventCounter = 0;
    private IDictionary<Guid, EventStream> _store = new Dictionary<Guid, EventStream>();
    // public IReadOnlyDictionary<Guid, EventStream> Streams => new ReadOnlyDictionary<Guid, EventStream>(_store);


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
        return _store[id].Events.Select(x => x.Metadata).ToArray();
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
          _store.Add(record.Id, EventStream.Create(record.Id, EventCounter + 1));
        }

        foreach(var e in record.Events)
        {
          _store[record.Id].Add(_mapper.GetEventName(e), e);
          EventCounter ++;
        }
        
        // inform projection manager to update projections
        _afterSave?.Invoke("store.afterSave", record.Events);
      }
    }

    public bool TryGet<T>(Guid id, out object[] events) {
      if(_store.ContainsKey(id) == false) {
        events = null;
        return false;
      }
      else
      {
        events = _store[id].Events.Select(x => x.Metadata).ToArray();
        return true;
      }
    }

    public long LatestVersion<T>(T record) where T : Aggregate
    {
      if(_store.ContainsKey(record.Id))
        return _store[record.Id].EventCount - 1;
      return -1;
    }

    // public object[] GetPreviousVersion<T>(Guid id, int versionOffset = 1)
    // {
    //   if(_store.TryGetValue(id, out var record)) {
    //     var eventCount = record.EventCount;
    //     versionOffset = (versionOffset >= eventCount) ? eventCount - 1 : versionOffset;
    //     return record.Events.SkipLast(versionOffset).ToArray();
    //   }
    //   return Empty;
    // }

    public void AfterSave(EventHandler<object[]> handler)
    {
      _afterSave += handler;
    }

    public void AfterDBReload(EventHandler<object[]> handler) {
      _afterDBReload += handler;
    }

    public void YAML_Load(string raw)
    {
      throw new NotImplementedException();
      // var stream = _serializer.Deserialize<IDictionary<Guid, IList<Event>>>(raw);
      // foreach(var events in stream.Values)
      // {
        // _afterDBReload?.Invoke("store.afterDbReload", events.Select(x => x.Metadata).ToArray());
      // }
      // _store = stream;
    }

    public string YAML_Export()
    {
      return _serializer.Serialize(_store.Values);
      // return _serializer.Serialize(new ReadOnlyDictionary<Guid, IList<Event>>(_store));
    }
  }
}