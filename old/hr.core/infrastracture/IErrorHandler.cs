using hr.core.helper;

namespace hr.core.infrastracture {
    public interface IErrorHandler<T> : IHandler<T> where T : ErrorEvent
    {
        
    }
}