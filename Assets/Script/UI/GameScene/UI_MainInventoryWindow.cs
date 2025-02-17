using Extensions;
using System.Linq;
using UnityEngine;


namespace Survive3D.Item {
    public class UI_MainInventoryWindow : UI_InventoryWindowBase {
        private WeaponInventoryData weaponInventoryData;

        UI_SlotItem weaponSlot;

        protected override void Awake() {
            base.Awake();

            inventoryData = new InventoryData(14);
            weaponInventoryData = new WeaponInventoryData();
            var slotTFs = root.Find("bg/Items").GetAllChilds();
            slots = new System.Collections.Generic.List<UI_SlotItem>(slotTFs.Length);

            for (int i = 0; i < slotTFs.Length; i++) {
                var slot = slotTFs[i].GetComponent<UI_SlotItem>();
                slot.Init(i, this);
                slots.Add(slot);
            }
            slotTFs = null;

            weaponSlot = root.Find<UI_SlotItem>("bg/UI_WeaponSlot");
            weaponSlot.Init(slots.Count, this,true);
        }

        public override void SetItem(int index, ItemData itemData) {
            if (index == inventoryData.ItemData.Length) {
                weaponInventoryData.SetWeaponItem(itemData);
                weaponSlot.SetItem(itemData);
            } else
                base.SetItem(index, itemData);
        }

        public override void RemoveItem(int index) {
            if (index == inventoryData.ItemData.Length) {
                //ÊÇÎäÆ÷À¸
                weaponInventoryData.RemoveWeaponItem();
                weaponSlot.SetItem(null);
            } else {
                base.RemoveItem(index);
            }
        }

        public override int GetItemCount(int configID) {
            return base.GetItemCount(configID);
        }

        public void UseItem(int index) {

        }


    }
}