using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Extensions {
    public static class TransformExtension {
        public static T Find<T>(this Transform transform, string path) where T : Component {
            if (transform == null)
                return null;
            var targetTF = transform.Find(path);
            return targetTF?.GetComponent<T>();
        }

        public static void BindListener(this Button button, UnityAction action) {
            if (button == null || action == null)
                return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        public static Transform[] GetAllChilds(this Transform transform) {
            if (!transform)
                return null;

            int childCount = transform.childCount;
            Transform[] childs = new Transform[childCount];
            for (int i = 0; i < childCount; i++) {
                childs[i] = transform.GetChild(i);
            }
            return childs;
        }

        public static void DestroyAllChilds(this Transform transform) {
            if (transform == null)
                return;

            var childs = transform.GetAllChilds();
            if (childs == null)
                return;
            foreach (var child in childs) {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        public static void SetActive(this Transform transform, bool active) {
            if (transform == null)
                return;

            transform.gameObject.SetActive(active);
        }
    }
}