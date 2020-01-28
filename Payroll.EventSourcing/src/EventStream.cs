using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using YamlDotNet.Serialization;

namespace Payroll.EventSourcing
{
  public class EventStreamPlaceholder
  {
    public Guid AggregateId { get; set; }
    public long StartLocation { get; set; }
    public IList<Event> Events { get; set;}
  }

  public class EventStream
  {
    public Guid AggregateId { get; private set; }
    public long StartLocation { get; private set; } = -1;
    private IList<Event> _events = new List<Event>();

    public static EventStream Create(Guid aggregateId, long location)
    {
      return new EventStream {
        AggregateId = aggregateId,
        StartLocation = location
      };
    }

    public void Add(string name, object meta)
    {
      _events.Add(Event.Create(StartLocation, _events.Count + 1, name, meta));
    }

    // public IEnumerable<object> Stream => _events.Select(x => x.Metadata).ToArray();
    
    public IReadOnlyCollection<Event> Events => new ReadOnlyCollection<Event>(_events);

    [YamlIgnore]
    public int EventCount => _events.Count;
  }
}