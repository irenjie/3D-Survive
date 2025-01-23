using MTLFramework.Math;
using System.Collections.Generic;

namespace Survive3D.Map {
    public class MapData {
        // 当前地图ID
        public ulong CurrentID = 1;
        // 玩家走过的地图块
        public List<SerializationVector2> MapChunkIndexList = new();
    }

    public class MapInitData {
        public int mapSize;
        public int mapSeed;
        public int spawnSeed;
        public float marshLimit;
    }


}