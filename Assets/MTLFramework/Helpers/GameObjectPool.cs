using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace MTLFramework.Helper {
    public static class GameObjectPool {
        /// <summary>
        /// key：地址，value：集合
        /// </summary>
        private static Dictionary<string, GameObjectCollection> gameObjectCollections =
            new Dictionary<string, GameObjectCollection>();

        public static void ClearAll() {
            lock (gameObjectCollections) {
                foreach (var collection in gameObjectCollections) {
                    collection.Value.RemoveAll();
                }
                gameObjectCollections.Clear();
            }
        }

        public static GameObject GetItem(string address) {
            lock (gameObjectCollections) {
                if (gameObjectCollections.TryGetValue(address, out var item)) {
                    return item.GetItem();
                }
                return null;
            }
        }

        public static void Recycle(Poolable gameObject) {
            if (gameObject == null)
                return;
            string address = gameObject.GetAddress();
            lock (gameObjectCollections) {
                if (gameObjectCollections.TryGetValue(address, out var item)) {
                    item.Recycle(gameObject.GetGameObject());
                }
            }
        }

        public static void Create(string address, int capacity) {
            lock (gameObjectCollections) {
                if (gameObjectCollections.ContainsKey(address)) {
                    return;
                }

                GameObjectCollection collection = new GameObjectCollection(address);
                collection.Expand(capacity);
                gameObjectCollections.Add(address, collection);
            }
        }


        private sealed class GameObjectCollection {
            Stack<GameObject> objects = new Stack<GameObject>();
            string address;
            // 增长大小
            const int fillSize = 5;

            public GameObjectCollection(string address) {
                this.address = address;
            }

            public GameObject GetItem() {
                lock (objects) {
                    if (objects.Count == 0) {
                        Expand(fillSize);
                    }
                    return objects.Pop();
                }
            }

            public void Expand(int count) {
                GameObject prefab = LoaderHelper.Get().GetAsset<GameObject>(address);
                for (int i = 0; i < count; i++) {
                    GameObject go = Object.Instantiate(prefab);
                    go.SetActive(false);
                    objects.Push(go);
                }
            }

            public void Recycle(GameObject target) {
                lock (objects) {
                    if (objects.Contains(target)) {
                        return;
                    }
                    objects.Push(target);
                }
            }

            public void Remove(int count) {
                lock (objects) {
                    while (count > 0 && objects.Count > 0) {
                        Object.Destroy(objects.Pop());
                    }
                }
            }

            public void RemoveAll() {
                lock (objects) {
                    while (objects.Count > 0) {
                        Object.Destroy(objects.Pop());
                    }
                }
            }

        }
    }

    public interface Poolable {
        string GetAddress();
        GameObject GetGameObject();
    }
}