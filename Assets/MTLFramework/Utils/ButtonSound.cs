using MTLFramework.Config;
using MTLFramework.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MTLFramework.Utils {
    public class ButtonSound : MonoBehaviour, IPointerClickHandler {
        Button button;
        [SerializeField] AudioClip clip;

        private void Awake() {
            button = transform.GetComponent<Button>();
            if (clip == null)
                clip = AudioConfig.Load().ButtonSound;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (button == null || !button.interactable)
                return;
            AudioHelper.Get().Play(clip);
        }
    }
}