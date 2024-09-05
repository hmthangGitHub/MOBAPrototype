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
        public ConfigHeroContainer.Config HeroConfig { get; set; }
        public ReactiveProperty<int> Level { get; set; } = new(1);
        public ReactiveProperty<int> SkillPointRemaining { get; set; } = new(1);
        public SkillModel[] SkillModels { get; set; } = Array.Empty<SkillModel>();
        public ReactiveProperty<int> TalentTree { get; set; } = new(1);
        public ReactiveProperty<float> Hp { get; set; } = new(1000);
        public ReactiveProperty<float> HpRegen { get; set; } = new(10);
        public ReactiveProperty<float> Mana { get; set; } = new(1000);
        public ReactiveProperty<float> ManaRegen { get; set; } = new(10.0f);
        public ReactiveProperty<float> MaxHp { get; set; } = new(1000);
        public ReactiveProperty<float> MaxMana { get; set; } = new(1000);
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
        public ReactiveProperty<float> CoolDownTimeStamp { get; set; } = new();
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