namespace hris.xunit.units.Serialization
{
    public interface IYAMLSerializable
    {
        void YAML_Load(string raw);
        string YAML_Export();
    }
}