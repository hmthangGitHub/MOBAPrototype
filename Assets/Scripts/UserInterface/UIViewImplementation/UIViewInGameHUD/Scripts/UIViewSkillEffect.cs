using System;
using System.Collections;
using System.Collections.Generic;
using UIView;
using UniRx;
using UnityEngine;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewSkillEffect : UIViewBase<UIViewSkillEffect.UIModel>
    {
        public enum SkillEffectType
        {
            Damage,
            Slow,
            Stun,
            DamagePerSecond
        }

        [Serializable]
        public class UIModel
        {
            [field: SerializeField] public ReactiveProperty<SkillEffectType> SkillEffectType { get; set; }
            [field: SerializeField] public ReactiveProperty<string> Value { get; set; }
        }
        
        [field: SerializeField] public UIViewFormatTextMeshProUGUI[] SkillCastTypes { get; set; }
        
        protected override void OnSetModel(UIModel model)
        {
            for (int i = 0; i < SkillCastTypes.Length; i++)
            {
                SkillCastTypes[i].gameObject.SetActive(i == (int)model.SkillEffectType.Value);
                SkillCastTypes[i].SetModel(model.Value);
            }
        }
    }
}