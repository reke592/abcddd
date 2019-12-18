using System;
using NHibernate;

namespace hr.com.infrastracture.database.nhibernate {
    public class NHSessionContext
    {
        [ThreadStatic]
        private static NHSessionContext _instance;

        [ThreadStatic]
        private static int _context_user;

        [ThreadStatic]
        private static object _session;

        [ThreadStatic]
        private static bool _stateless = false;

        [ThreadStatic]
        private static ISessionFactory _bind;

        public static NHSessionContext getInstance() {
            if(_context_user == 0) {
                _instance = new NHSessionContext();
            }
            _context_user ++;
            return _instance;
        }

        public bool IsStateless {
            get {
                return _stateless;
            }
        }

        public T GetSession<T>() {
            if(_session == null)
                throw new Exception($"No Open Session: {typeof(T).Name} in current context.");
            return (T) _session;
        }

        public ISession GetSession() {
            if(_session == null)
                throw new Exception($"no session in current context.");
            return (ISession) _session;
        }

        public IStatelessSession GetStatelessSession() {
            if(_session == null)
                throw new Exception($"no session in current context.");
            return (IStatelessSession) _session;
        }

        public bool hasBind(ISessionFactory factory) {
            return object.ReferenceEquals(_bind, factory);
        }

        public void Bind(ISession session) {
            _session = session;
            _bind = session.SessionFactory;
            _stateless = false;
        }

        public void BindStateless(IStatelessSession session, ISessionFactory factory) {
            _session = session;
            _bind = factory;
            _stateless = true;
        }

        public void UnBind(ISessionFactory factory) {
            if(object.ReferenceEquals(_bind, factory)) {
                _bind = null;
                _session = null;
                _context_user = 0;
                _stateless = false;
            }
        }
    }
}