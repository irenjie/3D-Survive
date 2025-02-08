using MTLFramework.Helper;
using Survive3D.MapObject;
using System.Collections.Generic;
using UnityEngine;

namespace Survive3D.Map {
    public class MapChunkController : MonoBehaviour {
        public Vector2Int chunkIndex { get; private set; }
        public Vector3 centerPosition { get; private set; }
        public bool isAllForest { get; private set; }
        public MapChunkData mapChunkData { get; private set; }

        private bool IsInitialized = false;
        private bool isActive = false;

        List<MapObjectData> mapObjectDatas = new();
        List<GameObject> realMapObjects = new();

        public void Init(Vector2Int chunkIndex, Vector3 centerPosition, bool isAllForest, MapChunkData mapChunkData) {
            this.chunkIndex = chunkIndex;
            this.centerPosition = centerPosition;
            this.isAllForest = isAllForest;
            this.mapChunkData = mapChunkData;

            IsInitialized = true;

        }

        public void SetActive(bool active) {
            if (isActive == active)
                return;

            isActive = active;
            gameObject.SetActive(active);

            if (active) {
                foreach (var mapObjectData in mapObjectDatas) {
                    InstantiateMapObject(mapObjectData);
                }
            } else {
                foreach (var mapObject in realMapObjects) {
                    GameObjectPool.Recycle(mapObject);
                }
            }
        }

        #region 地图对象相关

        public void AddMapObject(MapObjectData mapObjectData) {
            InstantiateMapObject(mapObjectData);
            mapObjectDatas.Add(mapObjectData);
        }

        private void InstantiateMapObject(MapObjectData mapObjectData) {
            var GO = GameObjectPool.GetItem(mapObjectData.mapObjectConfig.Prefab);
            GO.transform.parent = transform;
            GO.transform.position = mapObjectData.Position;
            realMapObjects.Add(GO);
        }

        #endregion
    }
}