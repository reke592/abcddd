using System;

namespace hr.helper.domain {
    public abstract class DomainObject {

        protected void registerCommandListener() {
            EventBroker.getInstance.addCommandListener(this.onCommand);
        }

        protected void registerQueryListener() {
            EventBroker.getInstance.addQueryListener(this.onQuery);
        }

        protected virtual void onCommand(object sender, Command c) {
            throw new NotImplementedException();
        }

        protected virtual void onQuery(object sender, Query q) {
            throw new NotImplementedException();
        }
    }
}