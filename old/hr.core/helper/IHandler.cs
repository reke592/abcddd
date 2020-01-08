namespace hr.core.helper {
    public interface IHandler<T> {
        void handle(object sender, T args);
    }
}