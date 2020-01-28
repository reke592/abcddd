namespace Payroll.EventSourcing
{
    public class Event
    {
        public long StartLocation { get; private set; }
        public long Index { get; private set; }
        public string Name { get; private set; }
        public string MetaVersion { get; private set; }
        public object Metadata { get; private set; }

        public static Event Create(long start, long index, string name, object meta)
        {
            return new Event
            {
                StartLocation = start,
                Index = index,
                Name = name,
                Metadata = meta,
                MetaVersion = meta.GetType().DeclaringType.Name
            };
        }
    }
}