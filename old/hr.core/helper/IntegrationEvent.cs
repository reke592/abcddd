using System;
using hr.core.domain;

namespace hr.core.helper {
    public enum Integration {
        CREATED,
        UPDATED,
        DELETED
    }

    public abstract class IntegrationEvent : Event {
        public object Data { get; private set; }
        public long EntityId { get; private set; }
        public Integration Integration { get; private set; }
        public Type TypeDTO { get; private set; }
        public Type EntityType { get; private set; }

        public override string ToString() {
            return $"{EntityType};{EntityId};{Integration}";
        }

        public IntegrationEvent(Entity data, Type dto, Integration integration) {
            EntityType = data.GetType();
            EntityId = data.Id;
            Data = data.Actual;
            TypeDTO = dto;
            Integration = integration;
        }
    }
}