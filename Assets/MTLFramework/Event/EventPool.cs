using MTLFramework.Event;
using System;
using System.Collections.Generic;

namespace MTLFramework.Event {
    public class EventPool<T> where T : GameEventArgs {
        private readonly Dictionary<EventID, EventHandler<T>> _dic = new Dictionary<EventID, EventHandler<T>>();

        public void Subscribe(EventID id, EventHandler<T> handler) {
            if (handler == null)
                return;

            if (_dic.TryGetValue(id, out EventHandler<T> handlers)) {
                // ·ÀÖ¹ÖØ¸´×¢²á
                var invocationList = handlers.GetInvocationList();
                foreach (var invocation in invocationList) {
                    if (invocation == handler)
                        return;
                }
                handlers += handler;
            } else {
                _dic.Add(id, handler);
            }
        }

        public void UnSubscribe(EventID id, EventHandler<T> handler) {
            if (handler != null && _dic.TryGetValue(id, out EventHandler<T> handlers)) {
                handlers -= handler;
            }
        }

        public bool Contains(EventID id, EventHandler<T> handler) {
            if (handler == null || !_dic.TryGetValue(id, out EventHandler<T> handlers))
                return false;

            var invocationList = handlers.GetInvocationList();
            foreach (var invocation in invocationList) {
                if (invocation == handler)
                    return true;
            }

            return false;
        }

        public void Fire(Object sender, EventID id, T args) {
            _dic.TryGetValue(id, out EventHandler<T> handlers);
            if (handlers != null) {
                handlers(sender, args);
            }
        }
    }
}
