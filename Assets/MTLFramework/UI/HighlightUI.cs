using UnityEngine;

namespace MTLFramework.UI {
    public class HighlightUI {
        public static void Hightlight(GameObject gameObject, int sortingOrder = 30000) {
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;
        }

        public static void RemoveHightlight(GameObject gameObject) {
            if (gameObject.TryGetComponent<Canvas>(out Canvas canvas)) {
                GameObject.Destroy(canvas);
            }
        }
    }
}