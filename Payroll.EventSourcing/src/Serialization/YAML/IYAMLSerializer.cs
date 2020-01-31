namespace Payroll.EventSourcing.Serialization.YAML
{
    public interface IYAMLSerializer
    {
        string Serialize(object o);
        T Deserialize<T>(string raw);
    }
}