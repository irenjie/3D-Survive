using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTLFramework.Helper {
    /// <summary>
    /// µ¥ÀýÄ£Ê½
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonClass<T> where T : class, new() {
        private static T _instance;
        public static T Get() {
            if (_instance == null) {
                lock (typeof(T)) {
                    _instance = new T();
                }
            }

            return _instance;
        }

        public static void Dispose() {
            _instance = null;
        }
    }
}