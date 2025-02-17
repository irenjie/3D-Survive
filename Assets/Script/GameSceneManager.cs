using MTLFramework.Helper;
using MTLFramework.UI;
using Sirenix.OdinInspector;
using Survive3D.Item;
using Survive3D.TimeS;
using UnityEngine;

namespace Survive3D.GameScene {
    public class GameSceneManager : DelayBehaviour {
        UI_MainInventoryWindow inventoryWindow;

        private void Awake() {
            DontDestroyOnLoad(LoaderHelper.Get().InstantiatePrefab("Assets/Debug/DebugMenu.prefab"));
            TimeManager.Get().Init();
        }

        private void Start() {
            inventoryWindow = UIManager.Combat.Navigation<UI_MainInventoryWindow>("UI/UI_MainInventoryWindow.prefab");
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