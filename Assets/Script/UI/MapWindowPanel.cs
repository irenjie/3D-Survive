using Extensions;
using MTLFramework.Helper;
using MTLFramework.UI;
using Survive3D.Map;
using Survive3D.MapObject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Survive3D.UI {
    public class MapWindowPanel : BasePanel {
        ScrollRect scrollRect;
        private RectTransform content;
        private float contentSize;
        private RectTransform playerIcon;

        [SerializeField] private GameObject mapitemPrefab;
        [SerializeField] private GameObject mapIconPrerfab;
        // 所有地图对象Icon
        private Dictionary<ulong, Image> mapObjectIconDic = new Dictionary<ulong, Image>();
        // 所有地图对象Icon
        private Dictionary<Vector2Int, Image> mapChunkImageDic = new();
        private float mapChunkImageSize;    // 一个chunk的ui尺寸
        private int mapChunkSize;   // chunk边长
        private float mapSizeOnWorld;   // 3d 地图世界尺寸
        private Sprite forestSprite;

        private float minScale;     // ui 地图缩放
        private float maxScale = 10;

        public void Init(float mapSize, int mapChunkSize, float mapSizeOnWorld, Texture2D forestTex) {
            scrollRect = root.Find<ScrollRect>("Scroll View");
            content = scrollRect.transform.Find("Viewport/Content") as RectTransform;
            playerIcon = content.parent.Find("PlayerIcon") as RectTransform;

            scrollRect.onValueChanged.AddListener((value) => {
                playerIcon.anchoredPosition3D = content.anchoredPosition3D;
            });

            this.mapSizeOnWorld = mapSizeOnWorld;
            this.forestSprite = CreateMapSprite(forestTex);
            contentSize = mapSizeOnWorld * 10;
            content.sizeDelta = new Vector2(contentSize, contentSize);
            content.localScale = new Vector3(maxScale, maxScale, 1);
            mapChunkImageSize = contentSize / mapSize;
            minScale = 1050f / contentSize;
        }

        private void Update() {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll == 0)
                return;

            float scale = Mathf.Clamp(content.localScale.x + scroll, minScale, maxScale);
            content.localScale = new Vector3(scale, scale, 0);
        }

        /// <summary>
        /// 更新content 中心点，是鼠标缩放的中心为玩家坐标
        /// </summary>
        /// <param name="playerPos"></param>
        public void UpdatePivot(Vector3 playerPos) {
            float x = playerPos.x / mapSizeOnWorld;
            float y = playerPos.z / mapSizeOnWorld;
            content.pivot = new Vector2(x, y);
        }

        /// <summary>
        /// 玩家发现一个新的地图块，显示其ui地图
        /// </summary>
        /// <param name=""></param>
        public void AddMapChunk(Vector2Int chunkIndex, List<MapObjectData> mapObjectDic, Texture2D texture) {
            RectTransform mapChunkRT = Instantiate(mapitemPrefab, content).GetComponent<RectTransform>();
            mapChunkRT.anchoredPosition = new Vector2(chunkIndex.x * mapChunkImageSize, chunkIndex.y * mapChunkImageSize);
            mapChunkRT.sizeDelta = new Vector2(mapChunkImageSize, mapChunkImageSize);

            Image mapChunkImage = mapChunkRT.GetComponent<Image>();
            mapChunkImageDic.Add(chunkIndex, mapChunkImage);
            if (texture == null) {
                mapChunkImage.type = Image.Type.Tiled;
                float ratio = forestSprite.texture.width / mapChunkImageSize;
                mapChunkImage.pixelsPerUnitMultiplier = mapChunkSize * ratio;
                mapChunkImage.sprite = forestSprite;
            } else {
                mapChunkImage.sprite = CreateMapSprite(texture);
            }

            foreach (var item in mapObjectDic) {
                AddMapObjectIcon(item);
            }
        }

        Sprite CreateMapSprite(Texture2D texture) {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        public void AddMapObjectIcon(MapObjectData mapObjectData) {
            MapObjectConfig config = mapObjectData.mapObjectConfig;
            if (config.MapIconSprite == null || config.IconSize <= 0)
                return;
            GameObject iconGO = GameObjectPool.GetItem(mapIconPrerfab);
            iconGO.transform.SetParent(content);
            Image iconImg = iconGO.GetComponent<Image>();
            iconImg.sprite = config.MapIconSprite;
            iconImg.transform.localScale = Vector3.one * config.IconSize;
            float x = mapObjectData.Position.x * 10;
            float y = mapObjectData.Position.z * 10;
            iconGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            mapObjectIconDic.Add(mapObjectData.ID, iconImg);

        }

        public void RemoveMapObjectIcon(ulong mapObjectID) {
            if (mapObjectIconDic.TryGetValue(mapObjectID, out Image iconImg)) {
                GameObjectPool.Recycle(iconImg.gameObject);
                mapObjectIconDic.Remove(mapObjectID);
            }
        }

        public void ResetWindow() {
            foreach (var item in mapObjectIconDic.Values) {
                GameObjectPool.Recycle(item.gameObject);
            }

            foreach (var item in mapChunkImageDic.Values) {
                Destroy(item.gameObject);
            }
            mapObjectIconDic.Clear();
            mapChunkImageDic.Clear();
        }

        public void SetActive(bool active) {
            if (gameObject.activeSelf == active)
                return;
            gameObject.SetActive(active);
        }

    }
}