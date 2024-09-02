using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIView;
using UniRx;
using UnityEngine;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewInGameHUD : UIViewBase<UIViewInGameHUD.UIModel>
    {
        [Serializable]
        public class UIModel
        {
            [field : SerializeField] public ReactiveProperty<string> HeroName { get; set; } = new();
            [field : SerializeField] public ReactiveCollection<UIViewSkill.UIModel> SkillLists { get; set; } = new();
        }
        
        [field : SerializeField] public UIViewSkillList SkillLists { get; set; }
        [field : SerializeField] public TextMeshProUGUI HeroName { get; private set; }

        protected override void OnSetModel(UIModel model)
        {
            model.HeroName.Subscribe(val => HeroName.text = val).AddTo(disposables);
            SkillLists.SetModel(model.SkillLists);
        }
    }
}