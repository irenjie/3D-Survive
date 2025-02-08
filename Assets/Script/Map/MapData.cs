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
        // 当前地图ID
        public ulong CurrentID = 1;
        // 玩家走过的地图块
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
    /// 地图块上一个对象的数据
    /// </summary>
    [Serializable]
    public class MapObjectData : IReference {
        // 唯一的身份标识
        public ulong ID;
        // 当前到底是什么配置
        public MapObjectConfig mapObjectConfig;
        // 剩余的腐烂天数，-1代表无效
        public int DestoryDays;
        // 坐标
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