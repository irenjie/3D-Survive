using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTLFramework.Helper {

    public class SingletonBehaviour<T> : DelayBehaviour where T : MonoBehaviour {
        private static T _instance;

        public static T Get() {
            if (_instance == null) {
                lock (typeof(T)) {
                    var gameObject = new GameObject(typeof(T).Name);
                    _instance = (T)gameObject.AddComponent(typeof(T));
                    DontDestroyOnLoad(gameObject);
                }
            }

            return _instance;
        }

        public void Dispose() {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy() {
            _instance = null;
        }
    }
}