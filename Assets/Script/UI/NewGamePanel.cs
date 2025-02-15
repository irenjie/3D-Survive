using Extensions;
using MTLFramework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Survive3D.UI {
    public class NewGamePanel : BasePanel {

        private void Start() {
            root.Find<Button>("Back_Button").onClick.AddListener(() => UIManager.Front.Close(this));
        }


    }
}