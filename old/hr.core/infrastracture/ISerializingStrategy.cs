namespace hr.core.infrastracture {
    public interface ISerializingStrategy {
        string Serialize(object o);
        object Deserialize(string str);
    }
}