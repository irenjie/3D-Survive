using Extensions;
using MTLFramework.Event;
using MTLFramework.UI;
using Survive3D.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Survive3D.UI {
    public class UI_MainInfoPanel : BasePanel {
        PlayerConfig playerConfig;

        Image dayNightImg;
        Text dayNumText;
        Image hungryImg;
        Image hpImg;

        [SerializeField] Sprite daySprite;
        [SerializeField] Sprite nightSprite;

        protected override void Awake() {
            base.Awake();
            playerConfig = PlayerConfig.Get();

            dayNightImg = root.Find<Image>("TimeState_Image");
            dayNumText = dayNightImg.transform.Find<Text>("Text");
            hungryImg = root.Find<Image>("Hungry_Image");
            hpImg = root.Find<Image>("Hp_Image");
            RegisterEvent();
        }

        void RegisterEvent() {
            EventManager.Get().Subscribe(EventID.DayNightChange, UpdateTimeState);
            EventManager.Get().Subscribe(EventID.NewDay, UpdateDayNum);
            EventManager.Get().Subscribe(EventID.PlayerHPUpdate, UpdatePlayerHP);
            EventManager.Get().Subscribe(EventID.PlayerHungryUpdate, UpdatePlayerHungry);
        }

        void UnsubscriptEvent() {
            EventManager.Get().UnSubscribe(EventID.DayNightChange, UpdateTimeState);
            EventManager.Get().UnSubscribe(EventID.NewDay, UpdateDayNum);
            EventManager.Get().UnSubscribe(EventID.PlayerHPUpdate, UpdatePlayerHP);
            EventManager.Get().UnSubscribe(EventID.PlayerHungryUpdate, UpdatePlayerHungry);
        }

        void UpdateTimeState(object sender, GameEventArgs args) {
            UpdateTimeState((args as BoolEventArgs).status);
        }

        void UpdateDayNum(object sender, GameEventArgs args) {
            UpdateDayNum((args as IntEventArgs).intArgs);
        }
        void UpdatePlayerHP(object sender, GameEventArgs args) {
            UpdatePlayerHP((args as FloatEventArgs).floatArgs);
        }
        void UpdatePlayerHungry(object sender, GameEventArgs args) {
            UpdatePlayerHungry((args as FloatEventArgs).floatArgs);
        }

        void UpdateTimeState(bool isDay) {
            dayNightImg.sprite = isDay ? daySprite : nightSprite;
        }

        void UpdateDayNum(int dayNum) {
            dayNumText.text = $"Day {dayNum + 1}";
        }

        void UpdatePlayerHP(float hp) {
            hpImg.fillAmount = hp / playerConfig.MaxHp;
        }

        void UpdatePlayerHungry(float hungry) {
            hungryImg.fillAmount = hungry / playerConfig.MaxHungry;
        }

        private void OnDestroy() {
            UnsubscriptEvent();
        }

    }
}