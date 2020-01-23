namespace Payroll.EventSourcing.Serialization
{
    public interface IYAMLSerializer
    {
        string Serialize(object o);
        T Deserialize<T>(string raw);
    }
}