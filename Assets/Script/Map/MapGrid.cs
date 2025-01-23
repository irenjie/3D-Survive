
using System.Collections.Generic;
using UnityEngine;

namespace Survive3D.Map {

    /// <summary>
    /// 地图网格，包含顶点和格子
    /// 10*10的地图，边有11个顶点，顶点保存内部9*9，格子保存10*10
    /// </summary>
    public class MapGrid {
        public Dictionary<Vector2Int, MapVertex> vertexDic = new();
        public Dictionary<Vector2Int, MapCell> cellDic = new();

        public int mapHeight { get; private set; }
        public int mapWidth { get; private set; }
        public float cellSize { get; private set; }

        public MapGrid(int mapHieght, int mapWidth, float cellSize) {
            (this.mapHeight, this.mapWidth, this.cellSize) = (mapHieght, mapWidth, cellSize);

            for (int x = 1; x < mapWidth; x++) {
                for (int z = 1; z < mapHeight; z++) {
                    AddVertex(x, z);
                    AddCell(x, z);
                }
            }

            for (int x = 1; x <= mapWidth; x++) {
                AddCell(x, mapHeight);
            }
            for (int z = 1; z <= mapHeight; z++) {
                AddCell(mapWidth, z);
            }


        }

        #region 顶点
        private void AddVertex(int x, int z) {
            vertexDic.Add(new Vector2Int(x, z), new MapVertex() { position = new Vector3(x * cellSize, 0, z * cellSize) });
        }

        public MapVertex GetVertex(Vector2Int index) {
            vertexDic.TryGetValue(index, out var result);
            return result;
        }

        public MapVertex GetVertex(int x, int z) {
            return GetVertex(new Vector2Int(x, z));
        }

        public MapVertex GetVertexByWorldPosition(Vector3 position) {
            int x = Mathf.Clamp(Mathf.RoundToInt(position.x / cellSize), 1, mapWidth);
            int z = Mathf.Clamp(Mathf.RoundToInt(position.z / cellSize), 1, mapHeight);
            return GetVertex(x, z);
        }

        public void SetVertexType(Vector2Int vertexIndex, MapVertexType type) {
            MapVertex vertex = GetVertex(vertexIndex);
            if (vertex == null)
                return;

            vertex.vertexType = type;
            switch (type) {
                case MapVertexType.Marsh:
                    MapCell cell = GetLeftBottomMapCell(vertexIndex);
                    if (cell != null)
                        cell.TextureIndex += 1;
                    cell = GetRightBottomMapCell(vertexIndex);
                    if (cell != null)
                        cell.TextureIndex += 2;
                    cell = GetLeftTopMapCell(vertexIndex);
                    if (cell != null)
                        cell.TextureIndex += 4;
                    cell = GetRightTopMapCell(vertexIndex);
                    if (cell != null)
                        cell.TextureIndex += 8;
                    break;
                default:
                    break;
            }
        }

        public void SetVertexType(int x, int z, MapVertexType type) {
            SetVertexType(new Vector2Int(x, z), type);
        }
        #endregion

        #region 网格
        public void AddCell(int x, int z) {
            float offset = cellSize / 2;
            cellDic.Add(new Vector2Int(x, z), new MapCell() { Position = new Vector3(x * cellSize - offset, z * cellSize - offset) });
        }

        public MapCell GetCell(Vector2Int index) {
            cellDic.TryGetValue(index, out MapCell cell);
            return cell;
        }

        public MapCell GetCell(int x, int z) {
            return GetCell(new Vector2Int(x, z));
        }

        public MapCell GetLeftBottomMapCell(Vector2Int vertexIndex) {
            return GetCell(vertexIndex.x + 1, vertexIndex.y);
        }

        public MapCell GetRightBottomMapCell(Vector2Int vertexIndex) {
            return GetCell(vertexIndex.x + 1, vertexIndex.y);
        }

        public MapCell GetLeftTopMapCell(Vector2Int vertexIndex) {
            return GetCell(vertexIndex.x, vertexIndex.y + 1);
        }

        public MapCell GetRightTopMapCell(Vector2Int vertexIndex) {
            return GetCell(vertexIndex.x + 1, vertexIndex.y + 1);
        }
        #endregion

        public void CalculateMapVertexType(float[,] noiseMap, float limit) {
            int width = noiseMap.GetLength(0);
            int height = noiseMap.GetLength(1);

            for (int x = 1; x < width; x++) {
                for (int z = 1; z < height; z++) {
                    if (noiseMap[x, z] >= limit) {
                        SetVertexType(x, z, MapVertexType.Marsh);
                    } else {
                        SetVertexType(x, z, MapVertexType.Forset);
                    }
                }
            }
        }
    }

    public enum MapVertexType {
        None,
        Forset,
        Marsh
    }

    public class MapVertex {
        public Vector3 position;
        public MapVertexType vertexType;
        // 顶点上的对象ID，0 为空
        public ulong MapObjectID;
    }

    public class MapCell {
        // 位置为格子中心
        public Vector3 Position;
        public int TextureIndex;
    }
}