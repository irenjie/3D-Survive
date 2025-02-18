using MTLFramework.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTLFramework.Event {
    public class EventManager : SingletonClass<EventManager> {
        private EventPool<GameEventArgs> _eventPool;
        public EventManager() {
            _eventPool = new EventPool<GameEventArgs>();
        }

        public void Subscribe(EventID id, EventHandler<GameEventArgs> handler) {
            _eventPool.Subscribe(id, handler);
        }

        public void UnSubscribe(EventID id, EventHandler<GameEventArgs> handler) {
            _eventPool.UnSubscribe(id, handler);
        }

        public void Fire(System.Object sender, EventID id, GameEventArgs args) {
            _eventPool.Fire(sender, id, args);
        }
    }
}