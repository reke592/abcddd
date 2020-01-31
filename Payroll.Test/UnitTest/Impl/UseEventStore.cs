using System;
using Payroll.Domain;
using Payroll.EventSourcing;
using EventStore.ClientAPI;
using System.Linq;
using System.Collections.Generic;

namespace Payroll.Test.UnitTest.Impl
{
  public class UseEventStore : IEventStore
  {
    private EventHandler<object[]> _afterSave;
    private EventHandler<object[]> _afterDBReload;
    private IEventStoreConnection _conn;
    private ITypeMapper _typeMapper;
    private ISerializer _serializer;

    public UseEventStore(ITypeMapper typeMapper, ISerializer serializer)
    {
      _typeMapper = typeMapper;
      _serializer = serializer;
      _conn = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"), "PayrollAppUnitTest");
      _conn.ConnectAsync().Wait();
    }

    public void AfterDBReload(EventHandler<object[]> handler)
    {
      _afterDBReload += handler;
    }

    public void AfterSave(EventHandler<object[]> handler)
    {
      _afterSave += handler;
    }

    public object[] Get<T>(Guid id) where T : Aggregate
    {
      var streamEvents = new List<ResolvedEvent>();
      StreamEventsSlice currentSlice;
      long nextSlice = StreamPosition.Start;
      
      do
      {
        currentSlice = _conn.ReadStreamEventsForwardAsync(_stream<T>(id), 0, 100, false).Result;
        nextSlice = currentSlice.NextEventNumber;
        streamEvents.AddRange(currentSlice.Events);
      }
      while(!currentSlice.IsEndOfStream);
    
      return streamEvents.ConvertAll<object>(doc => _serializer
        .Deserialize(doc.Event.Data, _typeMapper.GetEventType(doc.Event.EventType)))
        .ToArray();
    }

    public object[] GetPreviousVersion<T>(Guid id, int versionOffset = 1)
    {
      throw new NotImplementedException();
    }

    public long LatestVersion<T>(T record) where T : Aggregate
    {
      var result = _conn.ReadStreamEventsBackwardAsync(_stream<T>(record.Id), StreamPosition.End, 1, false, null).Result;
      return result.LastEventNumber;
    }

    public void Save<T>(T record) where T : Aggregate
    {
      // throw new NotImplementedException();
      var events = record.Events.Select(e => new EventData(
        eventId: Guid.NewGuid(),
        type: _typeMapper.GetEventName(e),
        isJson: _serializer.isJSON,
        _serializer.Serialize(e),
        null
      ));

      _conn.AppendToStreamAsync($"{typeof(T).Name}@{record.Id.ToString()}", ExpectedVersion.Any, events).Wait();
    }

    public bool TryGet<T>(Guid id, out object[] events)
    {
      var streamEvents = new List<ResolvedEvent>();
      StreamEventsSlice currentSlice;
      long nextSlice = StreamPosition.Start;
      
      do
      {
        currentSlice = _conn.ReadStreamEventsForwardAsync(_stream<T>(id), 0, 100, false).Result;
        nextSlice = currentSlice.NextEventNumber;
        streamEvents.AddRange(currentSlice.Events);
      }
      while(!currentSlice.IsEndOfStream);
    
    
      if(streamEvents.Count > 0)
      {
        events = streamEvents.ConvertAll<object>(doc => _serializer
          .Deserialize(doc.Event.Data, _typeMapper.GetEventType(doc.Event.EventType)))
          .ToArray();
        return true;
      }
      else
      {
        events = new object[] { };
        return false;
      }
    }

    // helper functions
    // stream name
    private string _stream<T>(Guid id) => $"{typeof(T).Name}@{id}";
  }
}