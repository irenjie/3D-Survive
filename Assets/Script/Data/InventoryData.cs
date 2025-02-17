using System;
using UnityEngine;

namespace Survive3D.Item {
    /// <summary>
    /// �������·����ӣ�����
    /// </summary>
    [Serializable]
    public class InventoryData {
        public ItemData[] ItemData { get; protected set; }

        public InventoryData(int itemCount) {
            ItemData = new ItemData[itemCount];
        }

        public void SetItem(int index, ItemData item) {
            ItemData[index] = item;
        }

        public void RemoveItem(int index) {
            ItemData[index] = null;
        }
    }

    /// <summary>
    /// ������������
    /// </summary>
    public class WeaponInventoryData {
        public ItemData weaponSlotItemData { get; private set; }

        public void RemoveWeaponItem() {
            weaponSlotItemData = null;
        }

        public void SetWeaponItem(ItemData weaponSlotItemData) {
            this.weaponSlotItemData = weaponSlotItemData;
        }


    }
}