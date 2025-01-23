

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTLFramework.Helper {
    public class TimerManager : SingletonBehaviour<TimerManager> {
        private Dictionary<int, Timer> timers = new Dictionary<int, Timer>();
        private HashSet<int> deleteTimers = new HashSet<int>();

        private void Awake() {

        }

        public int AddTimer(object handle, float step = 1f, float delay = 0f, int loop = 1, bool callbefoteStep = false,
            Action onStep = null, Action onComplete = null) {
            int id = Guid.NewGuid().GetHashCode();
            Timer timer = new Timer(handle, id, step, delay, loop, callbefoteStep, onStep, onComplete, false);
            timers.Add(id, timer);
            return id;
        }

        public void StopTimer(int id) {
            if (timers.TryGetValue(id, out Timer timer)) {
                timer.Stop();
                timers.Remove(id);
            }
        }


        public void Update() {
            float deltaTime = Time.deltaTime;
            foreach (var pair in timers) {
                pair.Value.Update(deltaTime);
                if (pair.Value.isFinish) {
                    deleteTimers.Add(pair.Key);
                }
            }

            foreach (int id in deleteTimers) {
                timers.Remove(id);
            }
        }

        private void OnDestroy() {

        }
    }
}