using MTLFramework.Helper;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MTLFramework.UI {
    public class BasePanel : DelayBehaviour {
        public Canvas canvas { get; protected set; }
        public RectTransform root { get; protected set; }
        private GraphicRaycaster raycaster;
        [SerializeField] private bool _isFullScreen;
        public bool IsFullScreen => _isFullScreen;
        public int sortingOrder {
            get {
                return canvas.sortingOrder;
            }
            set {
                canvas.sortingOrder = value;
            }
        }

        protected virtual void Awake() {
            root = transform.Find("Canvas") as RectTransform;
            canvas = root.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            raycaster = root.GetComponent<GraphicRaycaster>();
        }

        public void SetGraphic(bool enable) {
            canvas.enabled = enable;
        }

        public void SetGraphicRaycast(bool enable) {
            raycaster.enabled = enable;
        }

        public void PlayShowEffect() {

        }

        public void PlayCloseEffect(UnityAction closeAction) {
            closeAction?.Invoke();
        }

    }
}