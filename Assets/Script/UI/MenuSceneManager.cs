using MTLFramework.Helper;
using MTLFramework.Helpers;
using MTLFramework.UI;
using Survive3D.UI;
using UnityEngine;

namespace Survive3D.Scene {
    public class MenuSceneManager : DelayBehaviour {
        private void Awake() {
            UIManager.Front.Navigation<MenuScenePanel>("UI/MenuScenePanel.prefab");

            AudioClip clip = LoaderHelper.Get().GetAsset<AudioClip>("Audio/BG/Ambiance_Firecamp_Medium_Loop_Mono.wav");
            AudioHelper.Get().Play(clip, -1);
        }
    }
}