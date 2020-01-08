using System;
using hr.core.helper;

namespace hr.core.infrastracture {
    public interface IMessageBroker<T> where T : EventArgs {
        void onReceive(object sender, T args);
        void send(object sender, IntegrationEvent args);
    }
}