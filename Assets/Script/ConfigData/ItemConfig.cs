using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Survive3D.Config {
    public enum ItemType {
        [LabelText("װ��")] Weapon,
        [LabelText("����Ʒ")] Consumable,
        [LabelText("����")] Material
    }

    public enum WeaponType {
        [LabelText("��ͷ")] Axe,
        [LabelText("��")] PickAxe,
        [LabelText("����")] Sickle,
    }

    [CreateAssetMenu(menuName = "Config/��Ʒ����")]
    public class ItemConfig : SerializedScriptableObject {

        [SerializeField, LabelText("ͼ��")] private int mConfigId = 0;
        public int ConfigID => mConfigId;
        [LabelText("����")] public string Name;
        [LabelText("����"), OnValueChanged(nameof(OnItemTypeChanged))] public ItemType ItemType;
        [LabelText("����"), MultiLineProperty] public string Description;
        [LabelText("ͼ��")] public Sprite Icon;

        [LabelText("����ר����Ϣ"), SerializeField] public IItemTypeInfo ItemTypeInfo;

        // �������޸ĵ�ʱ���Զ�����ͬ������Ӧ�е�ר����Ϣ
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
        [LabelText("��������")] public WeaponType WeaponType;
        [LabelText("��������Ԥ����")] public GameObject PrefabOnPlayer;
        [LabelText("��������")] public Vector3 PositionOnPlayer;
        [LabelText("������ת")] public Vector3 RotationOnPlayer;
        [LabelText("����״̬��")] public AnimatorOverrideController AnimatorController;
        [LabelText("������")] public float AttackValue;
        [LabelText("�������")] public float AttackDurabilityCost;  // һ�ι��������ĺ���
        [LabelText("��������")] public float AttackDistance;
        [LabelText("������Ч")] public AudioClip AttackAudio;
        [LabelText("������Ч")] public AudioClip HitAudio;
        [LabelText("����Ч��")] public GameObject HitEffect;
    }

    /// <summary>
    /// ����Ʒ������Ϣ
    /// </summary>
    [Serializable]
    public class ItemCosumableInfo : IItemTypeInfo {
        [LabelText("�ָ�����ֵ")] public float RecoverHP;
        [LabelText("�ָ�����ֵֵ")] public float RecoverHungry;
    }

    /// <summary>
    /// ����Ʒ������Ϣ
    /// </summary>
    [Serializable]
    public class ItemMaterialInfo : IItemTypeInfo {
    }
}