using UnityEngine;

namespace Survive3D.Map {
    public class MapGenerator {
        MapInitData mapInitData;
        MapData mapData;

        private MapGrid mapGrid;
        private Material marshMaterial;

        public MapGenerator(MapInitData mapInitData, MapData mapData, MapGrid mapGrid, Material marshMaterial) {
            this.mapInitData = mapInitData;
            this.mapData = mapData;
            this.mapGrid = mapGrid;
            this.marshMaterial = marshMaterial;
        }

        public void GenerateMapData() {
        }

        private Mesh GenerateMapMesh(int height, int width, float cellSize) {
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[] {
                new Vector3(0,0,0),
                new Vector3(0,0,height*cellSize),
                new Vector3(width*cellSize,0,height*cellSize),
                new Vector3(width*cellSize,0,0),
            };

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
    }
}