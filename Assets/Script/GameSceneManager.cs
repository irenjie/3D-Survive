using MTLFramework.Helper;
using MTLFramework.UI;
using Sirenix.OdinInspector;
using Survive3D.Config;
using Survive3D.TimeS;
using Survive3D.UI;
using UnityEngine;

namespace Survive3D.GameScene {
    public class GameSceneManager : DelayBehaviour {
        UI_MainInventoryWindow inventoryWindow;
        UI_MainInfoPanel mainInfoPanel;

        private void Awake() {
            DontDestroyOnLoad(LoaderHelper.Get().InstantiatePrefab("Assets/Debug/DebugMenu.prefab"));
            TimeManager.Get().Init();
        }

        private void Start() {
            inventoryWindow = UIManager.Combat.PopUp<UI_MainInventoryWindow>("UI/UI_MainInventoryWindow.prefab");
            mainInfoPanel = UIManager.Combat.PopUp<UI_MainInfoPanel>("UI/UI_MainInfoWindow.prefab");
        }

#if UNITY_EDITOR
        [SerializeField] ItemConfig itemConfig;
        [Button]
        void AddItem() {
            UnityDebugHelper.Log("ÃÌº”ŒÔ∆∑ " + itemConfig.name);
            inventoryWindow.AddItem(itemConfig);
        }
#endif
    }
}