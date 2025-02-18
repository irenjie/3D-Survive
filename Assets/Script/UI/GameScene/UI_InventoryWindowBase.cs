using MTLFramework.UI;
using Survive3D.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Survive3D.Config {
    public abstract class UI_InventoryWindowBase : BasePanel {
        protected InventoryData inventoryData;
        protected List<UI_SlotItem> slots;

        public bool AddItem(ItemConfig itemConfig) {
            switch (itemConfig.ItemType) {
                case ItemType.Weapon:
                    return TryAddItemForEmptySlot(itemConfig);
                case ItemType.Consumable:
                    return TryAddPileItem(itemConfig);
                case ItemType.Material:
                    return TryAddPileItem(itemConfig);
            }

            return false;
        }

        protected bool TryAddItemForEmptySlot(ItemConfig itemConfig) {
            int index = GetEmptySlot();
            if (index < 0)
                return false;
            SetItem(index, new ItemData(itemConfig));
            return true;
        }

        protected bool TryAddPileItem(ItemConfig itemConfig) {
            foreach (var slot in slots) {
                if (slot.itemData != null && slot.itemData.config.ConfigID == itemConfig.ConfigID) {
                    // 没有堆叠上限
                    var data = (slot.itemData.itemTypeData as PileItemTypeDataBase);
                    data.count++;
                    slot.UpdateCountTextView();
                    return true;
                }
            }
            return TryAddItemForEmptySlot(itemConfig);
        }

        int GetEmptySlot() {
            for (int i = 0; i < slots.Count; i++) {
                if (slots[i].itemData == null)
                    return i;
            }
            return -1;
        }

        public virtual void SetItem(int index, ItemData itemData) {
            inventoryData.SetItem(index, itemData);
            slots[index].SetItem(itemData);
        }

        public virtual void RemoveItem(int index) {
            inventoryData.RemoveItem(index);
            slots[index].SetItem(null);
        }

        /// <summary>
        /// 获取某个物品的数量
        /// </summary>
        /// <param name="itemConfig"></param>
        /// <returns></returns>
        public virtual int GetItemCount(int configID) {
            int count = 0;
            foreach (var item in inventoryData.ItemData) {
                if (item != null && item.config.ConfigID == configID) {
                    if (item.itemTypeData is PileItemTypeDataBase) {
                        count += (item.itemTypeData as PileItemTypeDataBase).count;
                    } else {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}