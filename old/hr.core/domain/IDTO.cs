using hr.core.helper;

namespace hr.core.domain {
    public interface IDTO<T> : IValidity {
        T ToModel();
    }
}