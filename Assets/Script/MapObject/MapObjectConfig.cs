using Sirenix.OdinInspector;
using Survive3D.Map;
using UnityEngine;

namespace Survive3D.MapObject {
    [CreateAssetMenu(fileName = "��ͼ��Ʒ����", menuName = "Config/��ͼ��Ʒ����")]
    public class MapObjectConfig : ScriptableObject {
        [Header("�յ� ��������Ʒ")]
        public bool IsEmpty = false;
        //[Header("���ڵĵ�ͼ��������")]
        public MapVertexType MapVertexType;
        [Header("���ɵ�Ԥ����")]
        public GameObject Prefab;
        [Header("Icon")]
        public Sprite MapIconSprite;
        [Header("UI��ͼIcon�ߴ�,0��������Icon")]
        public float IconSize = 1;
        [Header("��������,-1����������")]
        public int DestoryDays = -1;
        [Header("���ɸ��� Ȩ������")]
        public int Probability;

        [Header("����"), MultiLineProperty] public string Description;
    }
}