using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MTLFramework.Helper {
    public static class CoroutineHelper {
        private static Dictionary<float, WaitForSeconds> _waitForSeconds = new Dictionary<float, WaitForSeconds>();
        private static Dictionary<float, WaitForSecondsRealtime> _waitForSecondsRealtime = new Dictionary<float, WaitForSecondsRealtime>();

        public static WaitForSeconds WaitForSeconds(float seconds) {
            if (!_waitForSeconds.TryGetValue(seconds, out WaitForSeconds waitSeconds))
                _waitForSeconds.Add(seconds, waitSeconds = new WaitForSeconds(seconds));
            return waitSeconds;
        }

        public static WaitForSecondsRealtime WaitForSecondsRealTime(float seconds) {
            if (!_waitForSecondsRealtime.TryGetValue(seconds, out WaitForSecondsRealtime waitSeconds))
                _waitForSecondsRealtime.Add(seconds, waitSeconds = new WaitForSecondsRealtime(seconds));
            return waitSeconds;
        }
    }
}