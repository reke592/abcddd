namespace Payroll.EventSourcing.Serialization.YAML
{
    public interface IYAMLSerializable
    {
        void YAML_Load(string raw);
        string YAML_Export();
    }
}