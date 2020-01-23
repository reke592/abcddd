namespace Payroll.EventSourcing
{
    public class Event
    {
        public string Name { get; private set; }
        public string MetaVersion { get; private set; }
        public object Metadata { get; private set; }

        public Event() { }

        public Event(string name, object meta)
        {
            Name = name;
            Metadata = meta;
            MetaVersion = meta.GetType().DeclaringType.Name;
        }
    }
}