using System;
using System.Collections.Generic;
using MobaPrototype.Config;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Hero
{
    [Serializable]
    public class HeroEntityModel
    {
        [field: SerializeField] public ConfigHeroContainer.Config HeroConfig { get; set; }
        [field: SerializeField] public ReactiveProperty<int> Level { get; set; } = new(1);
        public ReactiveProperty<int> SkillPointRemaining { get; set; } = new(1);
        [field: SerializeField] public ReactiveProperty<int> TalentTree { get; set; } = new(1);
        [field: SerializeField] public SkillModel[] SkillModels { get; set; } = Array.Empty<SkillModel>();
    }

    public class SkillModel
    {
        public ConfigSkillContainer.Config ConfigSkill { get; set; }
        public ConfigSkillLevelContainer.Config ConfigSkillLevel { get; set; }
        public ConfigSkillEffectContainer.Config ConfigSkillEffectLevel { get; set; }
        public ReactiveProperty<int> Level { get; set; } = new(0);

        public ReactiveProperty<float> CastRange { get; set; } = new();
        public ReactiveProperty<float> Aoe { get; set; } = new();
        public ReactiveProperty<float> ManaCost { get; set; } = new();
        public ReactiveProperty<float> CoolDown { get; set; } = new();
        public ReactiveProperty<SkillCastType> SkillCastType { get; set; } = new();
        public ReactiveDictionary<int, SkillEffectModel> SkillEffectModels { get; set; } = new();
    }

    public class SkillEffectModel
    {
        public SkillEffectType SkillEffectType { get; set; }
        public float EffectValue { get; set; }
        public float EffectDuration { get; set; }
    }
}