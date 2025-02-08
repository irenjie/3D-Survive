using Extensions;
using MTLFramework.Helper;
using MTLFramework.Math;
using Sirenix.OdinInspector;
using Survive3D.MapObject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survive3D.Map {
    [Serializable]
    public class MapData {
        // ��ǰ��ͼID
        public ulong CurrentID = 1;
        // ����߹��ĵ�ͼ��
        public List<SerializationVector2> MapChunkIndexList = new();
    }

    [Serializable]
    public class MapInitData {
        public int mapSize;
        public int mapSeed;
        public int spawnSeed;
        public float marshLimit;
    }

    [Serializable]
    public class MapChunkData {
        public SerializedDictionary<ulong, MapObjectData> MapObjectDic;
        public SerializedDictionary<ulong, MapObjectData> AIDataDic;

        [NonSerialized] public List<MapVertex> ForestVertexList;
        [NonSerialized] public List<MapVertex> MarshVertexList;
    }

    /// <summary>
    /// ��ͼ����һ�����������
    /// </summary>
    [Serializable]
    public class MapObjectData : IReference {
        // Ψһ����ݱ�ʶ
        public ulong ID;
        // ��ǰ������ʲô����
        public MapObjectConfig mapObjectConfig;
        // ʣ��ĸ���������-1������Ч
        public int DestoryDays;
        // ����
        private SerializationVector3 position;
        public Vector3 Position {
            get => position.ConverToVector3();
            set => position = value.ConverToSVector3();
        }

        public void Reset() {
            mapObjectConfig = null;
            DestoryDays = 0;
        }
    }


}