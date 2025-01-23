

using System;
using UnityEngine;

namespace MTLFramework.Helper {
    public class UnityDebugHelper {
        public static void Log(System.Object message) {
#if Debug_Mode || Develop_Mode
            Debug.Log(message);
#endif
        }

        public static void LogWarning(System.Object message) {
#if Debug_Mode || Develop_Mode
            Debug.LogWarning(message);
#endif
        }

        public static void LogError(System.Object message) {
            Debug.LogError(message);
        }

        public static void LogException(System.Exception exception) {
            Debug.LogException(exception);
        }

        public static void LogAssertion(System.Object message) {
#if Debug_Mode || Develop_Mode
            Debug.LogAssertion(message);
#endif
        }

    }
}