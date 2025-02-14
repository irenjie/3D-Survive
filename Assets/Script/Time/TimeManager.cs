using MTLFramework.Helper;
using MTLFramework.Helpers;
using UnityEngine;

using Time = UnityEngine.Time;

namespace Survive3D.TimeS {


    public class TimeManager : SingletonBehaviour<TimeManager> {
        [SerializeField] private Light mainLight;
        TimeConfig timeConfig;
        int stateCount;
        int curStateIndex;
        int nextStateIndex => (curStateIndex + 1) % stateCount;

        [SerializeField] float calculateTime;
        [SerializeField] int dayNum;

        public void Init() {
            timeConfig = LoaderHelper.Get().GetAsset<TimeConfig>("Config/ ±º‰≈‰÷√.asset");
            stateCount = timeConfig.timeStateConfigs.Length;
            mainLight = GameObject.Find("Directional Light").GetComponent<Light>();
        }

        private void Update() {
            UpdateTime();
        }

        private void UpdateTime() {
            calculateTime += Time.deltaTime;

            if (calculateTime >= GetConfig(curStateIndex).durationTime) {
                EnterNextState();
            }

            GetConfig(curStateIndex).CalculateLight(calculateTime, GetConfig(nextStateIndex)
                , out Quaternion sunRota, out Color sunColor, out float sunIntensity, out float fogDensity);

            SetLight(sunRota, sunColor, sunIntensity, fogDensity);
        }

        private void EnterNextState() {
            curStateIndex = nextStateIndex;
            if (curStateIndex == 0)
                dayNum++;
            calculateTime = 0;

            var config = GetConfig(curStateIndex);
            RenderSettings.fog = config.fog;
            if (config.bgClip != null) {
                AudioHelper.Get().Play(config.bgClip, -1);
            }


        }

        private void SetLight(Quaternion sunRota, Color sunColor, float sunIntensity, float fogDensity) {
            RenderSettings.ambientIntensity = sunIntensity;
            mainLight.intensity = sunIntensity;
            mainLight.transform.rotation = sunRota;
            mainLight.color = sunColor;
            RenderSettings.fog = fogDensity > 0;
            if (RenderSettings.fog) {
                RenderSettings.fogDensity = fogDensity;
            }
        }

        private TimeStateConfig GetConfig(int index) {
            return timeConfig.timeStateConfigs[index];
        }



    }
}
