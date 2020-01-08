namespace hr.core.domain {
    public class EntityFactory<T> where T : Entity {
        public static T Create(DTOBase<T> dto) {
            return dto.ToModel();
        }
    }
}