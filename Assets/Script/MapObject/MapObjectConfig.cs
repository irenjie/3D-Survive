using Sirenix.OdinInspector;
using Survive3D.Map;
using UnityEngine;

namespace Survive3D.MapObject {
    [CreateAssetMenu(fileName = "地图物品配置", menuName = "Config/地图物品配置")]
    public class MapObjectConfig : ScriptableObject {
        [LabelText("空的 不生成物品")]
        public bool IsEmpty = false;
        [LabelText("所在的地图顶点类型")]
        public MapVertexType MapVertexType;
        [LabelText("生成的预制体")]
        public GameObject Prefab;
        [LabelText("Icon")]
        public Sprite MapIconSprite;
        [LabelText("UI地图Icon尺寸,0代表不生成Icon")]
        public float IconSize = 1;
        [LabelText("销毁天数,-1代表不会销毁")]
        public int DestoryDays = -1;
        [LabelText("生成概率 权重类型")]
        public int Probability;
        [LabelText("描述"), MultiLineProperty] public string Description;
    }
}