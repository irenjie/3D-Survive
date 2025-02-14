using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace Survive3D.TimeS {

    [Serializable]
    public class TimeStateConfig {
        // 持续时间
        public float durationTime;
        // 阳光强度
        public float sunIntensity;
        // 阳光颜色
        public Color sunColor;
        // 太阳的角度
        [OnValueChanged(nameof(SetRotation))]
        public Vector3 sunRotation;
        [HideInInspector]
        public Quaternion sunQuaternion;
        public bool fog;
        public AudioClip bgClip;

        private void SetRotation() {
            sunQuaternion = Quaternion.Euler(sunRotation);
        }

        /// <summary>
        /// 给定当前时间和下一状态，与下一状态插值返回当前光照信息
        /// </summary>
        /// <param name="curTime"></param>
        /// <param name="nextState"></param>
        /// <param name="sunRota"></param>
        /// <param name="sunColor"></param>
        /// <param name="sunIntensity"></param>
        public void CalculateLight(float curTime, TimeStateConfig nextState, out Quaternion sunRota, out Color sunColor, out float sunIntensity, out float fogDensity) {

            float ratio = 1f - (curTime / durationTime);
            sunRota = Quaternion.Slerp(this.sunQuaternion, nextState.sunQuaternion, ratio);
            sunColor = Color.Lerp(this.sunColor, nextState.sunColor, ratio);
            sunIntensity = Mathf.Lerp(this.sunIntensity, nextState.sunIntensity, ratio);

            if (fog) {
                if (nextState.fog)
                    fogDensity = 0.1f;
                else
                    fogDensity = nextState.fog ? 0.1f : 0.1f * (1 - ratio);
            } else {
                fogDensity = 0f;
            }
        }


    }

    [CreateAssetMenu(fileName = "时间配置", menuName = "Config/时间配置")]
    public class TimeConfig : ScriptableObject {
        public TimeStateConfig[] timeStateConfigs;
    }
}