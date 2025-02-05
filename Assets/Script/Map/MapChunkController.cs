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
        }
    }
}