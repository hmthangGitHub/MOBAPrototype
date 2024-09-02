using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UIView;
using UniRx;
using UnityEngine;

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
        }

        [field: SerializeField] private UIViewButton Button { get; set; }
        [field: SerializeField] private TextMeshProUGUI HotKey { get; set; }
        [field: SerializeField] private UIViewButton UpgradeButton { get; set; }
        [field: SerializeField] private UIViewSkillLevelList SkillLevels { get; set; }
        
        protected override void OnSetModel(UIModel model)
        {
            Button.SetModel(model.Button);
            SkillLevels.SetModel(model.SkillLevels);
            foreach (var skillLevel in model.SkillLevels)
            {
                skillLevel.CanUpgrade.Subscribe(_ =>
                {
                    UpgradeButton.gameObject.SetActive(model.SkillLevels.Any(x => x.CanUpgrade.Value));
                }).AddTo(disposables);
            }
            HotKey.text = model.HotKey.ToString();
        }

        private void Update()
        {
            if (Model != default && Input.GetKeyUp(Model.HotKey))
            {
                Model.Button.OnClick.Invoke();
            }
        }
    }
}