using Extensions;
using MTLFramework.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MTLFramework.DebugF {
    public class DebugMenu : MonoBehaviour {
        Button debugBtn;
        Transform content;
        Transform panel;

        private void Start() {
            debugBtn = gameObject.GetComponent<Button>();
            content = transform.Find("Content");
            panel = content.Find("Viewport/Panel");
            content.SetActive(false);

            debugBtn.BindListener(() => {
                content.SetActive(!content.gameObject.activeSelf);
            });

            #region TimeScale
            {
                Transform timeScaleTF = panel.Find("TimeScale");
                InputField input = timeScaleTF.Find<InputField>("Input");
                timeScaleTF.Find<Button>("Button").BindListener(() => {
                    if (float.TryParse(input.text, out float scale)) {
                        Time.timeScale = scale;
                    }
                });
            }
            #endregion

            #region ≤‚ ‘∞¥≈•
            {
                panel.Find<Button>("TestBtn/Button").BindListener(() => {

                });
            }
            #endregion

        }

    }
}