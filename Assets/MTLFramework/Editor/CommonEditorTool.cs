using UnityEditor;
using UnityEngine;

namespace MTLFramework.Editor {
    public class CommonEditorTool {
        /// <summary>
        /// ���� project ���ļ�·��
        /// </summary>
        [InitializeOnLoadMethod]
        static void OnProjectFileSelected() {
            EditorApplication.projectWindowItemOnGUI += (guid, rect) => {
                if (Selection.activeObject == null)
                    return;

                string select_path = AssetDatabase.GetAssetPath(Selection.activeObject);
                string select_guid = AssetDatabase.AssetPathToGUID(select_path);
                if (select_guid == guid && !string.IsNullOrEmpty(select_guid)) {
                    if (GUI.Button(new Rect(rect.width - 80, rect.y, 80, rect.height), "����·��")) {
                        GUIUtility.systemCopyBuffer = select_path;
                        Debug.LogFormat("·�� {0} �Ѹ��Ƶ�������", select_path);
                    }
                }
            };
        }

        /// <summary>
        ///  ���� Hierarchy �ж���·��
        /// </summary>
        [InitializeOnLoadMethod]
        static void OnHierarchyGameObjectSelect() {
            EditorApplication.hierarchyWindowItemOnGUI += (instanceID, rect) => {
                if (Selection.activeObject != null && instanceID == Selection.activeObject.GetInstanceID()) {
                    if (GUI.Button(new Rect(rect.width, rect.y, 70, rect.height), "����·��")) {
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
