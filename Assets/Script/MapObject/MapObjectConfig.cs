using Sirenix.OdinInspector;
using Survive3D.Map;
using UnityEngine;

namespace Survive3D.MapObject {
    [CreateAssetMenu(fileName = "��ͼ��Ʒ����", menuName = "Config/��ͼ��Ʒ����")]
    public class MapObjectConfig : ScriptableObject {
        [LabelText("�յ� ��������Ʒ")]
        public bool IsEmpty = false;
        [LabelText("���ڵĵ�ͼ��������")]
        public MapVertexType MapVertexType;
        [LabelText("���ɵ�Ԥ����")]
        public GameObject Prefab;
        [LabelText("Icon")]
        public Sprite MapIconSprite;
        [LabelText("UI��ͼIcon�ߴ�,0��������Icon")]
        public float IconSize = 1;
        [LabelText("��������,-1����������")]
        public int DestoryDays = -1;
        [LabelText("���ɸ��� Ȩ������")]
        public int Probability;
        [LabelText("����"), MultiLineProperty] public string Description;
    }
}