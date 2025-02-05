using Extensions;
using MTLFramework.Math;
using System;
using System.Collections.Generic;

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
        [NonSerialized] public List<MapVertex> ForestVertexList;
        [NonSerialized] public List<MapVertex> MarshVertexList;
    }


}