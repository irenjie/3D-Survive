using Survive3D.Config;
using System;
using UnityEngine;

namespace Survive3D.Data {
    [Serializable]
    public class ItemData {
        public ItemConfig config;
        public IItemTypeData itemTypeData;

        public ItemData(ItemConfig config) {
            this.config = config;
            // 根据物品的实际类型 来创建符合类型的动态数据
            switch (config.ItemType) {
                case ItemType.Weapon:
                    itemTypeData = new ItemWeaponData() {
                        Durability = 100
                    };
                    break;
                case ItemType.Consumable:
                    itemTypeData = new ItemConsumableData() {
                        count = 1
                    };
                    break;
                case ItemType.Material:
                    itemTypeData = new ItemMaterialData() {
                        count = 1
                    };
                    break;
            }
        }

    }

    /// <summary>
    /// 物品类型数据的接口
    /// </summary>
    public interface IItemTypeData { }

    [Serializable]
    public class ItemWeaponData : IItemTypeData {
        public float Durability = 100;  // 耐久度 默认给100
    }

    /// <summary>
    /// 可堆叠物品
    /// </summary>
    [Serializable]
    public abstract class PileItemTypeDataBase {
        public int count;
    }

    [Serializable]
    public class ItemConsumableData : PileItemTypeDataBase, IItemTypeData {
    }

    [Serializable]
    public class ItemMaterialData : PileItemTypeDataBase, IItemTypeData {
    }
}