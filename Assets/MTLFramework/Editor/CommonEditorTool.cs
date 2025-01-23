using UnityEditor;
using UnityEngine;

namespace MTLFramework.Editor {
    public class CommonEditorTool {
        /// <summary>
        /// 复制 project 中文件路径
        /// </summary>
        [InitializeOnLoadMethod]
        static void OnProjectFileSelected() {
            EditorApplication.projectWindowItemOnGUI += (guid, rect) => {
                if (Selection.activeObject == null)
                    return;

                string select_path = AssetDatabase.GetAssetPath(Selection.activeObject);
                string select_guid = AssetDatabase.AssetPathToGUID(select_path);
                if (select_guid == guid && !string.IsNullOrEmpty(select_guid)) {
                    if (GUI.Button(new Rect(rect.width - 80, rect.y, 80, rect.height), "复制路径")) {
                        GUIUtility.systemCopyBuffer = select_path;
                        Debug.LogFormat("路径 {0} 已复制到剪贴板", select_path);
                    }
                }
            };
        }

        /// <summary>
        ///  复制 Hierarchy 中对象路径
        /// </summary>
        [InitializeOnLoadMethod]
        static void OnHierarchyGameObjectSelect() {
            EditorApplication.hierarchyWindowItemOnGUI += (instanceID, rect) => {
                if (Selection.activeObject != null && instanceID == Selection.activeObject.GetInstanceID()) {
                    if (GUI.Button(new Rect(rect.width, rect.y, 70, rect.height), "复制路径")) {
                        GUIUtility.systemCopyBuffer = GetTranformPath(Selection.activeTransform);
                    }
                }
            };
        }

        static string GetTranformPath(Transform transform) {
            if (!transform.parent)
                return transform.name;
            return GetTranformPath(transform.parent) + "/" + transform.name;
        }
    }
}
