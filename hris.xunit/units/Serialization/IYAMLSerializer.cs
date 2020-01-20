namespace hris.xunit.units.Serialization
{
    public interface IYAMLSerializer
    {
        string Serialize(object o);
        T Deserialize<T>(string raw);
    }
}