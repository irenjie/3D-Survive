using Extensions;
using MTLFramework.UI;
using UnityEngine;
using UnityEngine.UI;


namespace Survive3D.UI {
    public class MenuScenePanel : BasePanel {

        protected override void Awake() {
            base.Awake();
            Transform btnsTF = root.Find("Buttons");

            btnsTF.Find<Button>("NewGame").onClick.AddListener(NewGame);
            btnsTF.Find<Button>("ContinueGame").onClick.AddListener(ContinueGame);
            btnsTF.Find<Button>("Exit").onClick.AddListener(ExitGame);

        }

        private void NewGame() {
            UIManager.Front.PopUp<NewGamePanel>("UI/NewGamePanel.prefab");
            
        }

        private void ContinueGame() {

        }

        private void ExitGame() {

        }
    }
}