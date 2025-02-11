using MTLFramework.Helper;
using MTLFramework.UI;
using Survive3D.MapObject;
using Survive3D.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survive3D.Map {
    //public class MapManager : SingletonBehaviour<MapManager> {
    public class MapManager : MonoBehaviour {
        // ����
        float mapSizeOnWorld;
        float chunkSizeOnWorld; // ��λ ��

        // ����
        MapConfig mapConfig;
        [SerializeField] MapInitData mapInitData;
        private Dictionary<MapVertexType, List<MapObjectConfig>> spawnMapObjectConfigDic = new();// ĳ�����Ϳ���������Щ��ͼ��������

        // ������
        MapGenerator mapGenerator;
        Dictionary<Vector2Int, MapChunkController> mapChunkDic;
        [SerializeField] Transform viewer;
        Vector3 lastViewPosition = Vector3.one * -1;
        [SerializeField] bool canUpdateChunk = false;
        List<MapChunkController> lastVisibleChunkList = new();


        private void Awake() {
            Init();
        }

        public void Init() {
            StartCoroutine(InitInternal());
        }

        private IEnumerator InitInternal() {
            mapConfig = LoaderHelper.Get().GetAsset<MapConfig>("Config/Map/MapConfig.asset");

            // ��ͼ��Ʒ��������
            spawnMapObjectConfigDic.Add(MapVertexType.Forset, new());
            spawnMapObjectConfigDic.Add(MapVertexType.Marsh, new());
            List<MapObjectConfig> allMapObjectConfigs = LoaderHelper.Get().GetAssetsByLabel<MapObjectConfig>("MapObject");
            foreach (var mapObject in allMapObjectConfigs) {
                spawnMapObjectConfigDic[mapObject.MapVertexType].Add(mapObject);
            }
            allMapObjectConfigs = null;

            mapGenerator = new MapGenerator(mapConfig, mapInitData, null, spawnMapObjectConfigDic);
            mapGenerator.GenerateMapData();
            mapChunkDic = new Dictionary<Vector2Int, MapChunkController>();
            chunkSizeOnWorld = mapConfig.mapChunkSize * mapConfig.cellSize;
            mapSizeOnWorld = chunkSizeOnWorld * mapInitData.mapSize;
            yield break;


        }

        private void Update() {
            UpdateVisibleChunk();

            if (Input.GetKeyUp(KeyCode.M)) {
                if (isMapShowing)
                    CloseMapPanel();
                else
                    ShowMapPanel();
            }

            if (isMapShowing) {
                UpdateMapUI();
            }
        }

        #region ��ͼ�����

        private void UpdateVisibleChunk() {
            if (viewer.position == lastViewPosition)
                return;
            lastViewPosition = viewer.position;

            if (!canUpdateChunk)
                return;

            DoUpdateVisubleChunk();
        }

        private void DoUpdateVisubleChunk() {
            Vector2Int currentChunkIndex = GetMapChunkIndexByWorldPosition(viewer.position);
            // �رղ���Ҫ��ʾ�ĵؿ�
            for (int i = lastVisibleChunkList.Count - 1; i >= 0; i--) {
                Vector2Int chunkIndex = lastVisibleChunkList[i].chunkIndex;
                if (Mathf.Abs(chunkIndex.x - currentChunkIndex.x) > mapConfig.viewDinstance
                    || Mathf.Abs(chunkIndex.y - currentChunkIndex.y) > mapConfig.viewDinstance) {
                    lastVisibleChunkList[i].SetActive(false);
                    lastVisibleChunkList.RemoveAt(i);
                }
            }

            int startX = currentChunkIndex.x - mapConfig.viewDinstance;
            int startY = currentChunkIndex.y - mapConfig.viewDinstance;
            // ����Ҫչʾ�ĵؿ�
            for (int x = 0; x < 2 * mapConfig.viewDinstance + 1; x++) {
                for (int y = 0; y < 2 * mapConfig.viewDinstance + 1; y++) {
                    Vector2Int chunkIndex = new Vector2Int(startX + x, startY + y);
                    if (mapChunkDic.TryGetValue(chunkIndex, out var chunk)) {
                        if (!lastVisibleChunkList.Contains(chunk)) {
                            lastVisibleChunkList.Add(chunk);
                            chunk.SetActive(true);
                        }
                    } else {
                        chunk = GenerateMapChunk(chunkIndex);
                    }
                }
            }
            canUpdateChunk = false;
            TimerManager.Get().AddTimer(this, 1, 0, 1, false, null, () => canUpdateChunk = true);

        }

        public MapChunkController GetMapChunkController(Vector2Int index) {
            mapChunkDic.TryGetValue(index, out var chunkController);
            return chunkController;
        }

        private Vector2Int GetMapChunkIndexByWorldPosition(Vector3 worldPostion) {
            int x = Mathf.Clamp(Mathf.FloorToInt(worldPostion.x / chunkSizeOnWorld), 0, mapInitData.mapSize);
            int y = Mathf.Clamp(Mathf.FloorToInt(worldPostion.z / chunkSizeOnWorld), 0, mapInitData.mapSize);
            return new Vector2Int(x, y);
        }

        public MapChunkController GetMapChunkByWorldPosition(Vector3 worldPostion) {
            return mapChunkDic[GetMapChunkIndexByWorldPosition(worldPostion)];
        }

        private MapChunkController GenerateMapChunk(Vector2Int index, MapChunkData mapChunkData = null) {
            if (index.x < 0 || index.y < 0 || index.x > mapInitData.mapSize - 1 || index.y > mapInitData.mapSize - 1)
                return null;
            MapChunkController chunkController = mapGenerator.GenerateMapChunk(index, transform, mapChunkData, () => {
                mapPanel_chunkUpdateList.Add(index);
            });
            mapChunkDic.Add(index, chunkController);
            return chunkController;
        }

        #endregion

        #region ��ͼ�������

        public void SpawnMapObject(MapChunkController mapChunkController, MapObjectConfig mapObjectConfig, Vector3 pos) {
            MapObjectData mapObjectData = mapGenerator.GenerateMapObjectData(mapObjectConfig, pos);
            if (mapObjectData == null)
                return;
            mapChunkController.AddMapObject(mapObjectData);
        }

        #endregion


        #region UI���
        private bool isMapShowing = false;
        // �����µ�ͼ���б�
        private List<Vector2Int> mapPanel_chunkUpdateList = new();
        private MapWindowPanel mapWindowPanel;

        /// <summary>
        /// �򿪵�ͼUI
        /// </summary>
        private void ShowMapPanel() {
            mapWindowPanel = UIManager.GetUIManager(3, "MapPanel").Navigation<MapWindowPanel>("Assets/Prefabs/UI/MapWindowPanel.prefab");
            mapWindowPanel.Init(mapInitData.mapSize, mapConfig.mapChunkSize, mapSizeOnWorld, mapConfig.forestTexutre);
            isMapShowing = true;
        }

        void UpdateMapUI() {
            foreach (Vector2Int chunkIndex in mapPanel_chunkUpdateList) {
                Texture2D texture = null;
                MapChunkController chunk = mapChunkDic[chunkIndex];
                if (!chunk.isAllForest) {
                    texture = (Texture2D)chunk.GetComponent<MeshRenderer>().material.mainTexture;
                }
                mapWindowPanel.AddMapChunk(chunkIndex, chunk.mapObjectDatas, texture);
            }
            mapPanel_chunkUpdateList.Clear();
            mapWindowPanel.UpdatePivot(viewer.position);
        }

        private void CloseMapPanel() {
            UIManager.Front.Close(mapWindowPanel);
            isMapShowing = false;
        }
        #endregion
    }
}
