using Extensions;
using MTLFramework.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Survive3D.Item {
    public class UI_SlotItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
        Image bgImage;
        Image iconImg;
        Text countText;

        private bool isWeaponSlot = false;
        public int index { get; private set; }
        public ItemData itemData { get; private set; }
        UI_InventoryWindowBase inventoryWindow;

        private void Awake() {
            bgImage = transform.GetComponent<Image>();
            iconImg = transform.Find<Image>("Icon");
            countText = iconImg.transform.Find<Text>("Count");
        }

        public void Init(int index, UI_InventoryWindowBase inventoryWindow, bool isWeaponSlot = false) {
            this.index = index;
            this.inventoryWindow = inventoryWindow;
            this.isWeaponSlot = isWeaponSlot;
        }

        private void Start() {

        }

        private void Update() {

        }

        public void SetItem(ItemData itemData) {
            this.itemData = itemData;
            if (itemData == null) {
                bgImage.color = Color.white;
                countText.gameObject.SetActive(false);
                iconImg.sprite = null;
                iconImg.gameObject.SetActive(false);
                return;
            }

            countText.gameObject.SetActive(true);
            iconImg.gameObject.SetActive(true);
            iconImg.sprite = itemData.config.Icon;
            UpdateCountTextView();

        }

        public void UpdateCountTextView() {
            if (itemData == null)
                return;

            switch (itemData.config.ItemType) {
                case ItemType.Weapon:
                    bgImage.color = Color.white;
                    countText.text = (itemData.itemTypeData as ItemWeaponData).Durability.ToString() + "%";
                    break;
                case ItemType.Consumable:
                    bgImage.color = new Color(0, 1, 0, 0.5f);
                    countText.text = (itemData.itemTypeData as ItemConsumableData).count.ToString();
                    break;

                case ItemType.Material:
                    bgImage.color = Color.white;
                    countText.text = (itemData.itemTypeData as ItemMaterialData).count.ToString();
                    break;

            }
        }

        #region 鼠标点击拖拽交互
        bool draging = false;
        public void OnBeginDrag(PointerEventData eventData) {
            if (itemData == null)
                return;
            draging = true;
            HighlightUI.Hightlight(iconImg.gameObject);
        }

        public void OnDrag(PointerEventData eventData) {
            if (!draging || itemData == null)
                return;
            iconImg.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!draging || itemData == null)
                return;
            draging = false;
            iconImg.rectTransform.localPosition = Vector2.zero;
            HighlightUI.RemoveHightlight(iconImg.gameObject);

            GameObject castGO = eventData.pointerCurrentRaycast.gameObject;
            if (castGO == null || castGO.tag != "itemSlot") {
                inventoryWindow.RemoveItem(index);
            } else {
                // 与格子交换
                UI_SlotItem swapSlot = castGO.GetComponent<UI_SlotItem>();
                if (swapSlot.index == index)
                    return;
                if (swapSlot.isWeaponSlot && !(this.itemData.itemTypeData is ItemWeaponData))
                    return;

                ItemData swapItem = swapSlot.itemData;
                if (isWeaponSlot && swapItem != null && !(swapItem.itemTypeData is ItemWeaponData))
                    return;

                swapSlot.SetItem(this.itemData);
                SetItem(swapItem);
            }


        }

        #endregion
    }
}
