using MTLFramework.Math;
using System.Collections.Generic;

namespace Survive3D.Map {
    public class MapData {
        // ��ǰ��ͼID
        public ulong CurrentID = 1;
        // ����߹��ĵ�ͼ��
        public List<SerializationVector2> MapChunkIndexList = new();
    }

    public class MapInitData {
        public int mapSize;
        public int mapSeed;
        public int spawnSeed;
        public float marshLimit;
    }


}