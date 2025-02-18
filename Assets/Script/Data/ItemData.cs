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
            // ������Ʒ��ʵ������ �������������͵Ķ�̬����
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
    /// ��Ʒ�������ݵĽӿ�
    /// </summary>
    public interface IItemTypeData { }

    [Serializable]
    public class ItemWeaponData : IItemTypeData {
        public float Durability = 100;  // �;ö� Ĭ�ϸ�100
    }

    /// <summary>
    /// �ɶѵ���Ʒ
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