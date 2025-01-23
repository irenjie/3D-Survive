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

        public void Subscribe(int id, EventHandler<GameEventArgs> handler) {
            _eventPool.Subscribe(id, handler);
        }

        public void UnSubscribe(int id, EventHandler<GameEventArgs> handler) {
            _eventPool.UnSubscribe(id, handler);
        }

        public void Fire(System.Object sender, int id, GameEventArgs args) {
            _eventPool.Fire(sender, id, args);
        }
    }
}