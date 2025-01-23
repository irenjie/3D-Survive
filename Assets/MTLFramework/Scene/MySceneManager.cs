using MTLFramework.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace MScene {
    public class MySceneManager : SingletonBehaviour<MySceneManager> {
        private readonly List<KeyValuePair<BaseScene, SceneInstance>> scenes = new();

        public void LoadScene<T>(string address, LoadSceneMode loadSceneMode, SceneTransition<T> transition) where T : BaseScene {
            StartCoroutine(LoadSceneInternal(address, loadSceneMode, transition));
        }

        private IEnumerator LoadSceneInternal<T>(string address, LoadSceneMode loadSceneMode, SceneTransition<T> transition) where T : BaseScene {
            var handle = Addressables.LoadSceneAsync(address, loadSceneMode, false);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded) {
                T targetScene = default;

                var GOs = handle.Result.Scene.GetRootGameObjects();
                foreach (var go in GOs) {
                    if (go.TryGetComponent<T>(out targetScene))
                        break;
                }

                if (loadSceneMode == LoadSceneMode.Single) {
                    scenes.Clear();
                } else {
                    if (scenes.Count > 0) {
                        var showingScene = scenes.Last();
                        var rootGOs = showingScene.Value.Scene.GetRootGameObjects();
                        foreach (var go in rootGOs) {
                            go.SetActive(false);
                        }
                    }
                }
                yield return handle.Result.ActivateAsync();
                scenes.Add(new KeyValuePair<BaseScene, SceneInstance>(targetScene, handle.Result));
                transition.RaiseTransitionEnd(targetScene);
            } else {
                UnityDebugHelper.LogError($"加载场景{address}失败");
            }
        }

        public void UnloadScene<T>(SceneTransition<T> sceneTransition) where T : BaseScene {
            StartCoroutine(UnLoadSceneInternal<T>(sceneTransition));
        }

        /// <summary>
        /// 卸载最后加载的场景
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneTransition"></param>
        /// <returns></returns>
        private IEnumerator UnLoadSceneInternal<T>(SceneTransition<T> sceneTransition) where T : BaseScene {
            var targetScene = scenes.Last();

            var handle = Addressables.UnloadSceneAsync(targetScene.Value);
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                scenes.RemoveAt(scenes.Count - 1);
                sceneTransition.RaiseTransitionEnd(targetScene.Key as T);
            }
        }
    }

    public struct SceneTransition<T> where T : BaseScene {
        public event System.Action<T> transitionEnd;

        public readonly void RaiseTransitionEnd(T targetScene) {
            transitionEnd?.Invoke(targetScene);
        }
    }
}