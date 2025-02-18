using MTLFramework.Event;
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
            timeConfig = LoaderHelper.Get().GetAsset<TimeConfig>("Config/时间配置.asset");
            stateCount = timeConfig.timeStateConfigs.Length;
            mainLight = GameObject.Find("Directional Light").GetComponent<Light>();

            // true时为白天
            EventManager.Get().Fire(this, EventID.DayNightChange, new BoolEventArgs(curStateIndex <= 1));
            EventManager.Get().Fire(this, EventID.NewDay, new IntEventArgs(dayNum));
        }

        private void Update() {
            UpdateTime();
        }

        private void UpdateTime() {
            calculateTime += Time.deltaTime;

            if (calculateTime >= GetConfig(curStateIndex).durationTime) {
                EnterNextState();
                EventManager.Get().Fire(this, EventID.DayNightChange, new BoolEventArgs(curStateIndex <= 1));
            }

            GetConfig(curStateIndex).CalculateLight(calculateTime, GetConfig(nextStateIndex)
                , out Quaternion sunRota, out Color sunColor, out float sunIntensity, out float fogDensity);

            SetLight(sunRota, sunColor, sunIntensity, fogDensity);
        }

        private void EnterNextState() {
            curStateIndex = nextStateIndex;
            if (curStateIndex == 0) {
                dayNum++;
                EventManager.Get().Fire(this, EventID.NewDay, new IntEventArgs(dayNum));
            }
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
