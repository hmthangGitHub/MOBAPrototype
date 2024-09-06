using System;
using System.Collections.Generic;
using System.Linq;
using MobaPrototype.Config;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Hero
{
    [Serializable]
    public class HeroEntityModel
    {
        public ConfigTalentTreeContainer.Config[] TalentTreeConfigs { get; set; }
        public ConfigHeroContainer.Config HeroConfig { get; set; }
        public ReactiveProperty<int> Level { get; set; } = new(1);
        public ReactiveProperty<int> SkillPointRemaining { get; set; } = new(1);
        public SkillModel[] SkillModels { get; set; } = Array.Empty<SkillModel>();
        public ReactiveProperty<float> Hp { get; set; } = new(1000);
        public ReactiveProperty<float> HpRegen { get; set; } = new(1);
        public ReactiveProperty<float> Mana { get; set; } = new(300);
        public ReactiveProperty<float> ManaRegen { get; set; } = new(2.0f);
        public ReactiveProperty<float> MaxHp { get; set; } = new(1000);
        public ReactiveProperty<float> MaxMana { get; set; } = new(300);
        public ReactiveCollection<int> TalentTree { get; set; } = new();
    }

    public class SkillModel
    {
        public ConfigSkillContainer.Config ConfigSkill { get; set; }
        public ConfigSkillLevelContainer.Config ConfigSkillAtLevel { get; set; }
        public ReactiveProperty<int> Level { get; set; } = new(0);
        public ReactiveProperty<float> CastRange { get; set; } = new();
        public ReactiveProperty<float> Aoe { get; set; } = new();
        public ReactiveProperty<float> ManaCost { get; set; } = new();
        public ReactiveProperty<float> CoolDown { get; set; } = new();
        public ReactiveProperty<float> CoolDownTimeStamp { get; set; } = new();
        
        public ReactiveProperty<float> BuffCastRange { get; set; } = new();
        public ReactiveProperty<float> BuffAoe { get; set; } = new();
        public ReactiveProperty<float> BuffManaCost { get; set; } = new();
        public ReactiveProperty<float> BuffCoolDown { get; set; } = new();
        public ReactiveProperty<SkillCastType> SkillCastType { get; set; } = new();
        
        public ReactiveDictionary<int, SkillEffectModel> SkillEffectModels { get; set; } = new();
        public ReactiveProperty<int> NumberProjectTile { get; set; } = new(1);
        public string SkillEntityPath { get; set; }
        public ReactiveCollection<SkillEffectModel> AddedSkillEffect { get; set; } = new();

        public SkillEffectModel[] AllSkillEffectModels => SkillEffectModels.Values.Concat(AddedSkillEffect).ToArray();
    }

    public class SkillEffectModel
    {
        public SkillEffectType SkillEffectType { get; set; }
        public ReactiveProperty<float> EffectValue { get; set; } = new();
        public ReactiveProperty<float> EffectDuration { get; set; } = new();
        public ReactiveProperty<float> BuffEffectValue { get; set; } = new();
        public ReactiveProperty<float> BuffEffectDuration { get; set; } = new();
        public ConfigSkillEffectLevelContainer.Config ConfigSkillEffectAtLevel { get; set; }
    }
}