
using System;
using System.Collections.Generic;

namespace MTLFramework.Helper {
    public class ReferencePool {
        private static readonly Dictionary<Type, ReferenceCollection> referenceCollections = new Dictionary<Type, ReferenceCollection>();

        public static void ClearAll() {
            lock (referenceCollections) {
                foreach (var referenceCollection in referenceCollections) {
                    referenceCollection.Value.RemoveAll();
                }
                referenceCollections.Clear();
            }
        }

        public static T Acquire<T>() where T : class, IReference, new() {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }

        public static void Release(IReference reference) {
            if (reference == null)
                return;
            Type type = reference.GetType();
            var referecnceCollection = GetReferenceCollection(type, false);
            if (referecnceCollection != null)
                referecnceCollection.Release(reference);
        }

        public static void Add<T>(int count) where T : class, IReference, new() {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }

        public static void Remove<T>(int count) where T : class, IReference {
            GetReferenceCollection(typeof(T)).Remove(count);
        }

        public static void RemoveAll<T>() where T : class, IReference {
            GetReferenceCollection(typeof(T)).RemoveAll();
        }

        private static ReferenceCollection GetReferenceCollection(Type referenceType, bool create = true) {
            if (referenceType == null)
                return null;

            ReferenceCollection referenceCollection = null;
            lock (referenceCollections) {
                if (!referenceCollections.TryGetValue(referenceType, out referenceCollection) && create) {
                    referenceCollection = new ReferenceCollection(referenceType);
                    referenceCollections.Add(referenceType, referenceCollection);
                }
            }

            return referenceCollection;

        }

        private sealed class ReferenceCollection {
            private readonly Queue<IReference> references;
            public readonly Type referenceType;
            // 正在使用的数量
            public int usingReferenceCount { get; private set; }
            // 获取过的数量
            public int acquireReferenceCount { get; private set; }
            public int releaseReferenceCount { get; private set; }
            public int addReferenceCount { get; private set; }
            public int removeReferenceCount { get; private set; }

            public ReferenceCollection(Type referecneType) {
                this.referenceType = referecneType;
                usingReferenceCount = 0;
                acquireReferenceCount = 0;
                releaseReferenceCount = 0;
                addReferenceCount = 0;
                removeReferenceCount = 0;
            }

            public T Acquire<T>() where T : class, IReference, new() {
                if (typeof(T) != referenceType) {
                    UnityDebugHelper.LogError("Type is invaild");
                    return null;
                }

                usingReferenceCount++;
                acquireReferenceCount++;
                lock (references) {
                    if (references.Count > 0) {
                        return (T)references.Dequeue();
                    }
                }

                addReferenceCount++;
                return new T();
            }

            public void Release(IReference reference) {
                reference.Reset();
                lock (references) {
                    if (references.Contains(reference)) {
                        UnityDebugHelper.LogWarning("The reference has already released");
                        return;
                    }
                    references.Enqueue(reference);
                }

                releaseReferenceCount++;
                usingReferenceCount--;
            }

            public void Add<T>(int count) where T : class, IReference, new() {
                if (typeof(T) != referenceType) {
                    UnityDebugHelper.LogError("Type is invaild");
                    return;
                }

                lock (references) {
                    addReferenceCount += count;
                    while (count > 0) {
                        references.Enqueue(new T());
                    }
                }
            }

            public void Remove(int count) {
                lock (references) {
                    if (count > references.Count)
                        count = references.Count;
                    removeReferenceCount += count;
                    while (count > 0)
                        references.Dequeue();
                }
            }

            public void RemoveAll() {
                lock (references) {
                    removeReferenceCount += references.Count;
                    references.Clear();
                }
            }
        }
    }


}