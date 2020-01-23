namespace Payroll.Domain {
    public class Entity<T> {
        public T Id { get; protected set; }
    }
}