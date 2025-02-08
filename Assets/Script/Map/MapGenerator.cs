
using MTLFramework.Helper;
using Survive3D.MapObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survive3D.Map {
    public class MapGenerator {
        MapInitData mapInitData;
        MapData mapData;
        MapConfig mapConfig;
        Dictionary<MapVertexType, List<MapObjectConfig>> spawnMapObjectConfigDic;
        private int mapObjectForestWeightTotal;
        private int mapObjectMarshWeightTotal;

        private MapGrid mapGrid;
        private Material marshMaterial;
        private Mesh chunkMesh;

        public MapGenerator(MapConfig mapConfig, MapInitData mapInitData, MapData mapData, Dictionary<MapVertexType, List<MapObjectConfig>> spawnMapObjectConfigDic) {
            this.mapConfig = mapConfig;
            this.mapInitData = mapInitData;
            this.mapData = mapData;
            this.spawnMapObjectConfigDic = spawnMapObjectConfigDic;
        }

        public void GenerateMapData() {
            Random.InitState(mapInitData.mapSeed);
            float[,] noiseMap = GenerateNoiseMap(mapInitData.mapSize * mapConfig.mapChunkSize, mapInitData.mapSize * mapConfig.mapChunkSize, mapConfig.noiseLacunarity);
            mapGrid = new MapGrid(mapInitData.mapSize * mapConfig.mapChunkSize, mapInitData.mapSize * mapConfig.mapChunkSize, mapConfig.cellSize);
            mapGrid.CalculateMapVertexType(noiseMap, mapInitData.marshLimit);
            mapConfig.mapMaterial.mainTexture = mapConfig.forestTexutre;
            mapConfig.mapMaterial.SetTextureScale("_MainTex", new Vector2(mapConfig.cellSize * mapConfig.mapChunkSize, mapConfig.cellSize * mapConfig.mapChunkSize));
            marshMaterial = new Material(mapConfig.mapMaterial);
            //marshMaterial.SetTextureScale("_MainTex", Vector2.one);
            marshMaterial.mainTextureScale = Vector2.one;

            chunkMesh = GenerateMapMesh(mapConfig.mapChunkSize, mapConfig.mapChunkSize, mapConfig.cellSize);

            Random.InitState(mapInitData.spawnSeed);

            List<MapObjectConfig> temp = spawnMapObjectConfigDic[MapVertexType.Forset];
            foreach (MapObjectConfig mapObject in temp) {
                mapObjectForestWeightTotal += mapObject.Probability;
            }
            temp = spawnMapObjectConfigDic[MapVertexType.Marsh];
            foreach (MapObjectConfig mapObject in temp) {
                mapObjectMarshWeightTotal += mapObject.Probability;
            }
            temp = null;


        }

        public MapChunkController GenerateMapChunk(Vector2Int chunkIndex, Transform parent, MapChunkData mapChunkData, System.Action callBackForMapTexture) {
            GameObject chunkObj = new GameObject($"Chunk_{chunkIndex}");
            MapChunkController chunkController = chunkObj.AddComponent<MapChunkController>();
            chunkObj.AddComponent<MeshFilter>().mesh = chunkMesh;

            bool allForest;
            Texture2D mapTexture;
            chunkController.StartCoroutine(GenerateChunkTexture(chunkIndex, (texture, isAllForest) => {
                allForest = isAllForest;
                if (allForest) {
                    chunkObj.AddComponent<MeshRenderer>().sharedMaterial = mapConfig.mapMaterial;
                } else {
                    mapTexture = texture;
                    Material material = new Material(marshMaterial);
                    material.mainTexture = texture;
                    chunkObj.AddComponent<MeshRenderer>().material = material;
                }
                callBackForMapTexture?.Invoke();

                Vector3 position = new Vector3(chunkIndex.x * mapConfig.mapChunkSize * mapConfig.cellSize, 0, chunkIndex.y * mapConfig.mapChunkSize * mapConfig.cellSize);
                chunkController.transform.position = position;
                chunkObj.transform.SetParent(parent);

                Vector3 chunkCenterPosition = position + new Vector3(mapConfig.mapChunkSize * mapConfig.cellSize / 2, 0, mapConfig.mapChunkSize * mapConfig.cellSize / 2);
                chunkController.Init(chunkIndex, chunkCenterPosition, allForest, mapChunkData);

                GenerateMapChunkData(chunkIndex, chunkController);

            }));

            return chunkController;
        }

        private Mesh GenerateMapMesh(int height, int width, float cellSize) {
            Mesh mesh = new Mesh();
            // 确认顶点
            mesh.vertices = new Vector3[] {
                new Vector3(0,0,0),
                new Vector3(0,0,height*cellSize),
                new Vector3(width*cellSize,0,height*cellSize),
                new Vector3(width*cellSize,0,0),
            };

            // 确认哪些点形成三角形
            mesh.triangles = new int[] {
                0, 1, 2,
                0,2,3
            };

            mesh.uv = new Vector2[] {
                new Vector3(0,0),
                new Vector3(0,1),
                new Vector3(1,1),
                new Vector3(1,0),
            };
            mesh.RecalculateNormals();

            return mesh;
        }

        private float[,] GenerateNoiseMap(int width, int height, float lacunarity) {
            lacunarity += 0.1f;
            float[,] noiseMap = new float[width, height];
            float offsetX = Random.Range(-10000f, 10000f);
            float offsetY = Random.Range(-10000f, 10000f);
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    noiseMap[x, y] = Mathf.PerlinNoise(x * lacunarity + offsetX, y * lacunarity + offsetY);
                }
            }

            return noiseMap;
        }

        /// <summary>
        /// 生成地图块贴图
        /// </summary>
        /// <param name="chunkIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator GenerateChunkTexture(Vector2Int chunkIndex, System.Action<Texture2D, bool> callback) {
            // 当前地块的偏移量 找到这个地图块具体的每一个格子
            int cellOffsetX = chunkIndex.x * mapConfig.mapChunkSize + 1;
            int cellOffsetY = chunkIndex.y * mapConfig.mapChunkSize + 1;

            // 若地图块内全是森林格子，直接返回森林贴图
            bool isAllForest = true;
            for (int y = 0; y < mapConfig.mapChunkSize; y++) {
                if (!isAllForest)
                    break;
                for (int x = 0; x < mapConfig.mapChunkSize; x++) {
                    MapCell cell = mapGrid.GetCell(x + cellOffsetX, y + cellOffsetY);
                    if (cell != null && cell.TextureIndex != 0) {
                        isAllForest = false;
                        break;
                    }
                }
            }

            Texture2D mapTexture = null;
            if (isAllForest) {
                mapTexture = mapConfig.forestTexutre;
            } else {
                int textureCellSize = mapConfig.forestTexutre.width;
                int textureSize = mapConfig.mapChunkSize * textureCellSize;
                mapTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGB24, false);

                for (int y = 0; y < mapConfig.mapChunkSize; y++) {
                    // 绘制一列格子像素
                    yield return null;
                    int pixelOffsetY = y * textureCellSize;
                    for (int x = 0; x < mapConfig.mapChunkSize; x++) {
                        int pixelOffsetX = x * textureCellSize;
                        int textureIndex = mapGrid.GetCell(x + cellOffsetX, y + cellOffsetY).TextureIndex;
                        for (int py = 0; py < textureCellSize; py++) {
                            // 绘制一列像素
                            for (int px = 0; px < textureCellSize; px++) {
                                if (textureIndex <= 0) {
                                    Color color = mapConfig.forestTexutre.GetPixel(px, py);
                                    mapTexture.SetPixel(px + pixelOffsetX, py + pixelOffsetY, color);
                                } else {
                                    Color color = mapConfig.marshTextures[textureIndex - 1].GetPixel(px, py);
                                    if (color.a < 1f) {
                                        mapTexture.SetPixel(px + pixelOffsetX, py + pixelOffsetY, mapConfig.forestTexutre.GetPixel(px, py));
                                    } else {
                                        mapTexture.SetPixel(px + pixelOffsetX, py + pixelOffsetY, color);

                                    }
                                }
                            }
                        }
                    }
                }

                mapTexture.filterMode = FilterMode.Point;
                mapTexture.wrapMode = TextureWrapMode.Clamp;
                mapTexture.Apply();
            }

            callback?.Invoke(mapTexture, isAllForest);

        }

        #region 地图对象

        void GenerateMapChunkData(Vector2Int chunkIndex, MapChunkController mapChunkController) {
            int offsetX = chunkIndex.x * mapConfig.mapChunkSize;
            int offsetY = chunkIndex.y * mapConfig.mapChunkSize;

            for (int x = 1; x < mapConfig.mapChunkSize; x++) {
                for (int y = 1; y < mapConfig.mapChunkSize; y++) {
                    MapVertex mapVertex = mapGrid.GetVertex(x + offsetX, y + offsetY);
                    MapObjectConfig mapObjectConfig = GetMapObjectConfigByWeight(mapVertex.vertexType);
                    if (!mapObjectConfig.IsEmpty) {
                        Vector3 position = mapVertex.position + new Vector3(Random.Range(-mapConfig.cellSize / 2, mapConfig.cellSize / 2), 0, Random.Range(-mapConfig.cellSize / 2, mapConfig.cellSize / 2));
                        MapObjectData mapObjectData = GenerateMapObjectData(mapObjectConfig, position);

                        mapChunkController.AddMapObject(mapObjectData);

                    }
                }
            }
        }

        /// <summary>
        /// 根据配置的概率获取一个地图对象
        /// </summary>
        /// <param name="mapVertexType"></param>
        /// <returns></returns>
        private MapObjectConfig GetMapObjectConfigByWeight(MapVertexType mapVertexType) {
            List<MapObjectConfig> mapObjectConfigs = spawnMapObjectConfigDic[mapVertexType];
            int weightTotal = mapVertexType == MapVertexType.Forset ? mapObjectForestWeightTotal : mapObjectMarshWeightTotal;
            int randomValue = Random.Range(1, weightTotal + 1);
            float temp = 0;
            foreach (var mapObjectConfig in mapObjectConfigs) {
                temp += mapObjectConfig.Probability;
                if (temp >= randomValue)
                    return mapObjectConfig;
            }

            return mapObjectConfigs[0];
        }

        public MapObjectData GenerateMapObjectData(MapObjectConfig mapObjectConfig, Vector3 pos) {
            if (mapObjectConfig.IsEmpty)
                return null;
            return GenerateMapObjectData(mapObjectConfig, pos, mapObjectConfig.DestoryDays);
        }

        public MapObjectData GenerateMapObjectData(MapObjectConfig mapObjectConfig, Vector3 position, int destroyDays) {
            MapObjectData mapObjectData = ReferencePool.Acquire<MapObjectData>();
            mapObjectData.mapObjectConfig = mapObjectConfig;
            //mapObjectData.ID = mapData.CurrentID;
            //++mapData.CurrentID;
            mapObjectData.Position = position;
            mapObjectData.DestoryDays = destroyDays;

            return mapObjectData;
        }

        #endregion
    }
}