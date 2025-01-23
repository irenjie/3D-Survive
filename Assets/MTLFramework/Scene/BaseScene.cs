using MTLFramework.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace MScene {

    /// <summary>
    /// load - begin - resume
    /// pause - finish
    /// pause - resume
    /// </summary>
    public abstract class BaseScene : DelayBehaviour {
        string sceneName;
        protected virtual void OnLoad() { }
        protected virtual void Begin() { }
        protected virtual void Resume() { }
        protected virtual void Pause() { }
        protected virtual void Finish() { }

    }
}