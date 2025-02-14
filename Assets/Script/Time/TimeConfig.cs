using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace Survive3D.TimeS {

    [Serializable]
    public class TimeStateConfig {
        // ����ʱ��
        public float durationTime;
        // ����ǿ��
        public float sunIntensity;
        // ������ɫ
        public Color sunColor;
        // ̫���ĽǶ�
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
        /// ������ǰʱ�����һ״̬������һ״̬��ֵ���ص�ǰ������Ϣ
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

    [CreateAssetMenu(fileName = "ʱ������", menuName = "Config/ʱ������")]
    public class TimeConfig : ScriptableObject {
        public TimeStateConfig[] timeStateConfigs;
    }
}