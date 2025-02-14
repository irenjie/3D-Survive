using MTLFramework.Helper;
using MTLFramework.UI;
using Survive3D.MapObject;
using Survive3D.Player;
using Survive3D.TimeS;
using Survive3D.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survive3D.Map {
    //public class MapManager : SingletonBehaviour<MapManager> {
    public class MapManager : MonoBehaviour {
        // 数据
        float mapSizeOnWorld;
        float chunkSizeOnWorld; // 单位 米
        MeshCollider meshCollider;

        // 配置
        MapConfig mapConfig;
        [SerializeField] MapInitData mapInitData;
        private Dictionary<MapVertexType, List<MapObjectConfig>> spawnMapObjectConfigDic = new();// 某个类型可以生成那些地图对象配置

        // 管理器
        MapGenerator mapGenerator;
        Dictionary<Vector2Int, MapChunkController> mapChunkDic;
        [SerializeField] Transform viewer;
        Vector3 lastViewPosition = Vector3.one * -1;
        [SerializeField] bool canUpdateChunk = false;
        List<MapChunkController> lastVisibleChunkList = new();


        private void Awake() {
            Init();
            GameObject.Find("Main Camera").GetComponent<CameraController>().Init(mapSizeOnWorld);
        }

        public void Init() {
            StartCoroutine(InitInternal());

            DontDestroyOnLoad(LoaderHelper.Get().InstantiatePrefab("Assets/Debug/DebugMenu.prefab"));
            viewer.GetComponent<PlayerController>().Init(mapSizeOnWorld);
            TimeManager.Get().Init();
        }

        private IEnumerator InitInternal() {

            mapConfig = LoaderHelper.Get().GetAsset<MapConfig>("Config/Map/MapConfig.asset");

            // 地图物品生成配置
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

            GetComponent<MeshCollider>().sharedMesh = GenerateGroundMesh(mapSizeOnWorld, mapSizeOnWorld);

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

        #region 地图块相关

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
            // 关闭不需要显示的地块
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
            // 开启要展示的地块
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

        #region 地图对象相关

        public void SpawnMapObject(MapChunkController mapChunkController, MapObjectConfig mapObjectConfig, Vector3 pos) {
            MapObjectData mapObjectData = mapGenerator.GenerateMapObjectData(mapObjectConfig, pos);
            if (mapObjectData == null)
                return;
            mapChunkController.AddMapObject(mapObjectData);
        }

        #endregion

        #region UI相关
        private bool isMapShowing = false;
        // 待更新地图块列表
        private List<Vector2Int> mapPanel_chunkUpdateList = new();
        private MapWindowPanel mapWindowPanel;

        /// <summary>
        /// 打开地图UI
        /// </summary>
        private void ShowMapPanel() {
            if (mapWindowPanel == null) {
                mapWindowPanel ??= UIManager.GetUIManager(3, "MapPanel").Navigation<MapWindowPanel>("Assets/Prefabs/UI/MapWindowPanel.prefab");
                mapWindowPanel.Init(mapInitData.mapSize, mapConfig.mapChunkSize, mapSizeOnWorld, mapConfig.forestTexutre);
            }
            mapWindowPanel.SetActive(true);
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
            mapWindowPanel?.SetActive(false);
            //UIManager.GetUIManager(3, "MapPanel").Close(mapWindowPanel);
            isMapShowing = false;
        }
        #endregion

        private Mesh GenerateGroundMesh(float height, float width) {
            Mesh mesh = new Mesh();
            // 确定顶点在哪里
            mesh.vertices = new Vector3[]{
                new Vector3(0,0,0),
                new Vector3(0,0,height),
                new Vector3(width,0,height),
                new Vector3(width,0,0),
            };
            // 确定哪些点形成三角形
            mesh.triangles = new int[]{
                0,1,2,
                0,2,3
            };
            mesh.uv = new Vector2[]{
                new Vector3(0,0),
                new Vector3(0,1),
                new Vector3(1,1),
                new Vector3(1,0),
            };
            return mesh;
        }
    }
}
