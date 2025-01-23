using MTLFramework.Event;
using System;
using System.Collections.Generic;

namespace MTLFramework.Event {
    public class EventPool<T> where T : GameEventArgs {
        private readonly Dictionary<int, EventHandler<T>> _dic = new Dictionary<int, EventHandler<T>>();

        public void Subscribe(int id, EventHandler<T> handler) {
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

        public void UnSubscribe(int id, EventHandler<T> handler) {
            if (handler != null && _dic.TryGetValue(id, out EventHandler<T> handlers)) {
                handlers -= handler;
            }
        }

        public bool Contains(int id, EventHandler<T> handler) {
            if (handler == null || !_dic.TryGetValue(id, out EventHandler<T> handlers))
                return false;

            var invocationList = handlers.GetInvocationList();
            foreach (var invocation in invocationList) {
                if (invocation == handler)
                    return true;
            }

            return false;
        }

        public void Fire(Object sender, int id, T args) {
            _dic.TryGetValue(id, out EventHandler<T> handlers);
            if (handlers != null) {
                handlers(sender, args);
            }
        }
    }
}
