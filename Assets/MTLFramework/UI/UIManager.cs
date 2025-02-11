using Extensions;
using MTLFramework.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MTLFramework.UI {

    public class UIManager : MonoBehaviour {
        #region ui层级
        // panel 间隔层级
        const ushort panelPaddingLayer = 100;
        // group 间隔层级
        const ushort groupPaddingLayer = 3000;
        // 当前 group（manager）order,group初始层级为 order x groupPaddingLayer
        ushort order;
        #endregion

        // 正在展示的页面
        readonly List<BasePanel> panels = new List<BasePanel>();

        #region 获取 UIManager
        static GameObject root;
        static readonly Dictionary<int, UIManager> managers = new Dictionary<int, UIManager>();
        // 战斗场景的常驻界面
        public static UIManager Combat => Get(1, "Combat");
        // 前排界面
        public static UIManager Front => Get(2, "Front");

        public static UIManager GetUIManager(ushort order, string name) {
            return Get(order, name);
        }

        private static UIManager Get(ushort order, string name) {
            if (!managers.ContainsKey(order)) {
                if (root == null) {
                    root = new GameObject("UI Managers");
                    DontDestroyOnLoad(root.gameObject);
                }

                var g = new GameObject(name);
                g.transform.parent = root.transform;
                UIManager manager = g.AddComponent<UIManager>();
                manager.order = order;
                managers.Add(order, manager);
            }
            return managers[order];
        }

        #endregion

        #region 页面开关
        /*
         * 页面开关时更新如下：
         * 页面层级
         * 页面遮罩层级与透明度
         * 背后界面的显示
         */

        /// <summary>
        /// 跳转页面
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <returns></returns>
        public T Navigation<T>(string address) where T : BasePanel {
            Clear();
            T panel = LoaderHelper.Get().InstantiatePrefab(address, transform).GetComponent<T>();
            panels.Add(panel);
            panel.PlayShowEffect();
            panel.sortingOrder = order * groupPaddingLayer;
            UpdateMask();
            return panel;
        }

        /// <summary>
        /// 弹出页面
        /// </summary>
        /// <returns></returns>
        public T PopUp<T>(string address) where T : BasePanel {
            var lastTopPanel = TopPanel;
            T panel = LoaderHelper.Get().InstantiatePrefab(address, transform).GetComponent<T>();
            panels.Add(panel);
            panel.PlayShowEffect();
            panel.sortingOrder = lastTopPanel == null ? order * groupPaddingLayer : lastTopPanel.sortingOrder + panelPaddingLayer;
            if (panel.IsFullScreen && lastTopPanel != null) {
                lastTopPanel.SetGraphic(false);
            }

            UpdateMask();
            return panel;
        }

        public void Close(BasePanel panel) {
            bool closingTop = panel == TopPanel;
            int panelCount = PanelCount;
            BasePanel newTopPanel = panels.TryGetAt<BasePanel>(panelCount - 2);

            if (closingTop) {
                if (panel.IsFullScreen && newTopPanel != null) {
                    newTopPanel.SetGraphic(true);
                }
            }

            panel.PlayCloseEffect(() => {
                Addressables.ReleaseInstance(panel.gameObject);
            });
            panels.RemoveAt(panelCount - 1);

            if (closingTop) {
                UpdateMask();
            }
        }

        public void Clear() {
            foreach (var panel in panels) {
                panel.PlayCloseEffect(() => {
                    Addressables.ReleaseInstance(panel.gameObject);
                });
            }
            UpdateMask();
            panels.Clear();
        }

        public static void ClearAll() {
            foreach (var manager in managers) {
                manager.Value.Clear();
            }
        }


        /// <summary>
        /// 更新层级与遮罩
        /// 当最上层页面非全屏时显示
        /// </summary>
        private void UpdateMask() {
            if (this != Front)
                return;

            int panelCount = PanelCount;

            if (panelCount > 0) {

                // 更新遮罩
                var lastPanel = panels.Last();
                if (!lastPanel.IsFullScreen) {
                    var panelLayer = lastPanel.sortingOrder;
                    UIMaskHelper.Get().UpdateLayer(panelLayer - 1);
                }
            } else {
                UIMaskHelper.Get().UpdateLayer(-1, 0f, false);
            }

        }
        #endregion

        #region 页面获取
        public ushort PanelCount => (ushort)panels.Count;
        public BasePanel TopPanel => panels.LastOrDefault();
        #endregion

    }
}