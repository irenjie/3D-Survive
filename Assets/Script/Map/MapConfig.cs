using Sirenix.OdinInspector;
using UnityEngine;

namespace Survive3D.Map {
    [CreateAssetMenu(fileName = "��ͼ����", menuName = "Config/��ͼ����")]
    public class MapConfig : ScriptableObject {
        [LabelText("һ����ͼ���м���")]
        public int mapChunkSize;
        [LabelText("һ�����Ӷ�����")]
        public float cellSize; 
        [LabelText("������϶")]
        public float noiseLacunarity;
        [LabelText("��ͼ����")]
        public Material mapMaterial;
        [LabelText("ɭ����ͼ")]
        public Texture2D forestTexutre;
        [LabelText("������ͼ")]
        public Texture2D[] marshTextures;
        [LabelText("��ҿ��Ӿ��룬��λ��Chunk")]
        public int viewDinstance;
        [LabelText("��ͼ�糿ˢ�¸��ʣ�1/x")]
        public int RefreshProbability;

        [Header("AI")]
        [LabelText("��ͼ��AI��������")]
        public int maxAIOnChunk;
        [LabelText("��ͼ��ɭ��/��������AI����С������")]
        public int GenerateAIMinVertexCountOnChunk;
    }
}