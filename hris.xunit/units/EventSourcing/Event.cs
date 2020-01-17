namespace hris.xunit.units.EventSourcing
{
    public class Event
    {
        public string Name { get; private set; }
        public object Metadata { get; private set; }

        public Event(string name, object meta)
        {
            Name = name;
            Metadata = meta;
        }
    }
}