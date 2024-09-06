using System;
using System.Collections;
using System.Collections.Generic;
using UIView;
using UniRx;
using UnityEngine;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewSkillInfoPopUp : UIViewBase<UIViewSkillInfoPopUp.UIModel>
    {
        public enum SkillCastType
        {
            Direction,
            TargetInstant,
            Aoe,
            TargetAoe,
            Target
        }

        [Serializable]
        public class UIModel
        {
            [field: SerializeField] public ReactiveProperty<string> Name { get; set; }
            [field: SerializeField] public ReactiveProperty<int> Level { get; set; }
            [field: SerializeField] public ReactiveProperty<SkillCastType> CastType { get; set; }

            [field: SerializeField] public ReactiveCollection<UIViewSkillEffect.UIModel> EffectList { get; set; }
            [field: SerializeField] public ReactiveProperty<string> Description { get; set; }
            [field: SerializeField] public ReactiveProperty<string> CoolDown { get; set; }
            [field: SerializeField] public ReactiveProperty<string> ManaCost { get; set; }
        }

        [field: SerializeField] public UIViewFormatTextMeshProUGUI Name { get; set; }
        [field: SerializeField] public UIViewFormatTextMeshProUGUI Level { get; set; }
        [field: SerializeField] public UIViewFormatTextMeshProUGUI CastType { get; set; }
        [field: SerializeField] public UIViewSkillEffectList EffectList { get; set; }
        [field: SerializeField] public UIViewFormatTextMeshProUGUI Description { get; set; }
        [field: SerializeField] public UIViewFormatTextMeshProUGUI CoolDown { get; set; }
        [field: SerializeField] public UIViewFormatTextMeshProUGUI ManaCost { get; set; }

        protected override void OnSetModel(UIModel model)
        {
            Name.SetModel(model.Name);
            Level.SetModel(model.Level);
            CastType.SetModel(model.CastType);
            EffectList.SetModel(model.EffectList);
            Description.SetModel(model.Description);
            CoolDown.SetModel(model.CoolDown);
            ManaCost.SetModel(model.ManaCost);
        }
    }
}