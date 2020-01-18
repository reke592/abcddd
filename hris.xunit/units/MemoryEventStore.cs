using System;
using System.Collections.Generic;
using System.Linq;
using hris.xunit.units.domain;
using hris.xunit.units.EventSourcing;

namespace hris.xunit.units
{
    public class MemoryEventStore : IEventStore
    {
        private TypeMapper _mapper;
        internal event EventHandler<object[]> afterSave;
        public object[] Empty => new object[] { };
        IDictionary<Guid, IList<Event>> _store = new Dictionary<Guid, IList<Event>>();

        public MemoryEventStore(TypeMapper mapper)
        {
            _mapper = mapper;
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
                if(!_store.ContainsKey(record.Id))
                    _store.Add(record.Id, new List<Event>());
                
                foreach(var e in record.Events)
                    _store[record.Id].Add(new Event(_mapper.GetEventName(e), e));
                
                // inform projection manager to update projections
                afterSave?.Invoke(this, record.Events);
            }
        }

        public long LatestVersion<T>(T record) where T : Aggregate
        {
            if(_store.ContainsKey(record.Id))
                return _store[record.Id].Count - 1;
            return -1;
        }
    }
}