using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UIView;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewSkill : UIViewBase<UIViewSkill.UIModel>
    {
        [Serializable]
        public class UIModel
        {
            [field: SerializeField] public UIViewButton.UIModel Button { get; set; }
            [field: SerializeField] public ReactiveCollection<UIViewSkillLevel.UIModel> SkillLevels { get; set; }
            [field: SerializeField] public UIViewButton.UIModel UpgradeButton { get; set; }
            [field: SerializeField] public KeyCode HotKey { get; set; }
            [field: SerializeField] public ReactiveProperty<float> CoolDownTimeStamp { get; set; }
            [field: SerializeField] public ReactiveProperty<float> CoolDownTotalTime { get; set; }
            [field: SerializeField] public ReactiveProperty<float> ManaCost { get; set; }
        }

        [field: SerializeField] private UIViewButton Button { get; set; }
        [field: SerializeField] private TextMeshProUGUI HotKey { get; set; }
        [field: SerializeField] private UIViewButton UpgradeButton { get; set; }
        [field: SerializeField] private UIViewSkillLevelList SkillLevels { get; set; }
        [field: SerializeField] private Image CoolDownClock { get; set; }
        [field: SerializeField] private TextMeshProUGUI CoolDownText { get; set; }
        [field: SerializeField] private TextMeshProUGUI ManaCost { get; set; }
        
        protected override void OnSetModel(UIModel model)
        {
            Button.SetModel(model.Button);
            UpgradeButton.SetModel(model.UpgradeButton);
            SkillLevels.SetModel(model.SkillLevels);
            foreach (var skillLevel in model.SkillLevels)
            {
                skillLevel.CanUpgrade.Subscribe(_ =>
                {
                    UpgradeButton.gameObject.SetActive(model.SkillLevels.Any(x => x.CanUpgrade.Value));
                }).AddTo(disposables);
            }
            HotKey.text = model.HotKey.ToString();
            model.ManaCost.Subscribe(val =>
            {
                ManaCost.text = Mathf.CeilToInt(val).ToString();
            });
        }

        private void Update()
        {
            if (Model == default) return;
            InvokeHotKeyClick();
            ShowCoolDown();
        }

        private void ShowCoolDown()
        {
            var remainingCoolDownTime = Model.CoolDownTimeStamp.Value - Time.time;
            CoolDownClock.gameObject.SetActive(remainingCoolDownTime > 0);
            CoolDownClock.fillAmount = Mathf.Max(0.0f, remainingCoolDownTime / Model.CoolDownTotalTime.Value);
            CoolDownText.text = Mathf.CeilToInt(remainingCoolDownTime).ToString();
        }

        private void InvokeHotKeyClick()
        {
            if (Input.GetKeyUp(Model.HotKey))
            {
                Model.Button.OnClick.Invoke();
            }
        }
    }
}