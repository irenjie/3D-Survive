using MTLFramework.Helper;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Survive3D.Data {
    [CreateAssetMenu(menuName = "Config/玩家配置", fileName = "玩家配置")]
    public class PlayerConfig : SerializedScriptableObject {
        #region 角色配置
        [FoldoutGroup("角色配置"), LabelText("旋转速度")]
        public float RotateSpeed = 10;
        [FoldoutGroup("角色配置"), LabelText("移动速度")]
        public float MoveSpeed = 4;
        [FoldoutGroup("角色配置"), LabelText("最大生命值")]
        public float MaxHp = 100;
        [FoldoutGroup("角色配置"), LabelText("最大饥饿值")]
        public float MaxHungry = 100;
        [FoldoutGroup("角色配置"), LabelText("饥饿值衰减速度")]
        public float HungryReduceSpeed = 0.2f;
        [FoldoutGroup("角色配置"), LabelText("当饥饿值为0的时 生命值的衰减速度")]
        public float HpReduceSpeedOnHungryIsZero = 2;
        //[FoldoutGroup("角色配置"), LabelText("走路音效")]
        //public AudioClip[] FootstepAudioClis;
        //[FoldoutGroup("角色配置"), LabelText("默认动画状态机")]
        //public RuntimeAnimatorController NormalAnimatorController;
        #endregion

        static PlayerConfig config;
        public static PlayerConfig Get() {
            config ??= LoaderHelper.Get().GetAsset<PlayerConfig>("Config/玩家配置.asset");
            return config;
        }
    }
}