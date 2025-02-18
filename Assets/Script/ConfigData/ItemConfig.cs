using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Survive3D.Config {
    public enum ItemType {
        [LabelText("装备")] Weapon,
        [LabelText("消耗品")] Consumable,
        [LabelText("材料")] Material
    }

    public enum WeaponType {
        [LabelText("斧头")] Axe,
        [LabelText("镐")] PickAxe,
        [LabelText("镰刀")] Sickle,
    }

    [CreateAssetMenu(menuName = "Config/物品配置")]
    public class ItemConfig : SerializedScriptableObject {

        [SerializeField, LabelText("图标")] private int mConfigId = 0;
        public int ConfigID => mConfigId;
        [LabelText("名称")] public string Name;
        [LabelText("类型"), OnValueChanged(nameof(OnItemTypeChanged))] public ItemType ItemType;
        [LabelText("描述"), MultiLineProperty] public string Description;
        [LabelText("图标")] public Sprite Icon;

        [LabelText("类型专属信息"), SerializeField] public IItemTypeInfo ItemTypeInfo;

        // 当类型修改的时候，自动生成同等类型应有的专属信息
        private void OnItemTypeChanged() {
            switch (ItemType) {
                case ItemType.Weapon:
                    ItemTypeInfo = new ItemWeaponInfo();
                    break;
                case ItemType.Consumable:
                    ItemTypeInfo = new ItemCosumableInfo();
                    break;
                case ItemType.Material:
                    ItemTypeInfo = new ItemMaterialInfo();
                    break;
            }
        }
    }

    public interface IItemTypeInfo { }

    [Serializable]
    public class ItemWeaponInfo : IItemTypeInfo {
        [LabelText("武器类型")] public WeaponType WeaponType;
        [LabelText("玩家手里的预制体")] public GameObject PrefabOnPlayer;
        [LabelText("武器坐标")] public Vector3 PositionOnPlayer;
        [LabelText("武器旋转")] public Vector3 RotationOnPlayer;
        [LabelText("动画状态机")] public AnimatorOverrideController AnimatorController;
        [LabelText("攻击力")] public float AttackValue;
        [LabelText("攻击损耗")] public float AttackDurabilityCost;  // 一次攻击带来的耗损
        [LabelText("攻击距离")] public float AttackDistance;
        [LabelText("攻击音效")] public AudioClip AttackAudio;
        [LabelText("命中音效")] public AudioClip HitAudio;
        [LabelText("命中效果")] public GameObject HitEffect;
    }

    /// <summary>
    /// 消耗品类型信息
    /// </summary>
    [Serializable]
    public class ItemCosumableInfo : IItemTypeInfo {
        [LabelText("恢复生命值")] public float RecoverHP;
        [LabelText("恢复饥饿值值")] public float RecoverHungry;
    }

    /// <summary>
    /// 消耗品类型信息
    /// </summary>
    [Serializable]
    public class ItemMaterialInfo : IItemTypeInfo {
    }
}