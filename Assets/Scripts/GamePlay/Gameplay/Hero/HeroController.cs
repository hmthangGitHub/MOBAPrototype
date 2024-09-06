using System;
using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using MobaPrototype.Config;
using MobaPrototype.SkillEntity;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype.Hero
{
    public interface IHeroController
    {
        public HeroEntityModel HeroEntityModel { get; }
        public HeroCommand HeroCommand { get; }
    }

    public class HeroController : LifetimeScope, IHeroController
    {
        [SerializeField] private CharacterAnimatorController characterAnimatorController;
        [SerializeField] private AOESkillPreviewer aoeSkillPreviewer;
        [SerializeField] private RangeSkillPreviewer rangeSkillPreviewer;
        [SerializeField] private TargetSkillPreviewer targetSkillPreviewer;
        
        [field: SerializeField] public HeroEntityModel HeroEntityModel { get; set; }
        [field: SerializeField] public int ConfigHeroKey { get; private set; } = 0;
        public HeroCommand HeroCommand { get; } = new();

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            InitializeHeroEntityModel(builder);
            builder.RegisterInstance(HeroCommand);
            builder.RegisterInstance(characterAnimatorController);
            builder.RegisterInstance(aoeSkillPreviewer);
            builder.RegisterInstance(rangeSkillPreviewer);
            builder.RegisterInstance(targetSkillPreviewer);
            builder.RegisterEntryPoint<HeroCommandExecutor>();
            builder.Register<HeroAoeSkillExecutor>(Lifetime.Singleton).AsSelf();
            builder.Register<HeroDirectionalSkillExecutor>(Lifetime.Singleton).AsSelf();
            builder.Register<HeroTargetSkillExecutor>(Lifetime.Singleton).AsSelf();
        }

        private void InitializeHeroEntityModel(IContainerBuilder builder)
        {
            var configHeroContainer = Parent.Container.Resolve<ConfigHeroContainer>();
            var configSkillContainer = Parent.Container.Resolve<ConfigSkillContainer>();
            var configSkillLevelContainer = Parent.Container.Resolve<ConfigSkillLevelContainer>();
            var configSkillEffectContainer = Parent.Container.Resolve<ConfigSkillEffectContainer>();
            var configSkillEffectLevelContainer = Parent.Container.Resolve<ConfigSkillEffectLevelContainer>();
            var configTalentTreeContainer = Parent.Container.Resolve<ConfigTalentTreeContainer>();
            var configTalentTreeEffectContainer = Parent.Container.Resolve<ConfigTalentTreeEffectContainer>();

            HeroEntityModel = new()
            {
                Level = new(1),
                HeroConfig = configHeroContainer.ConfigDictionary[ConfigHeroKey],
                TalentTree = CreateTalentTree(configTalentTreeContainer, configTalentTreeEffectContainer),
                SkillModels = configSkillContainer.GroupConfigLookUp[ConfigHeroKey].Select(x => CreateSkillModel(x, configSkillEffectContainer, configSkillLevelContainer, configSkillEffectLevelContainer)).ToArray(),
                TalentTreeConfigs = configTalentTreeContainer.GroupConfigLookUp[ConfigHeroKey].ToArray()
            };
            builder.RegisterInstance(HeroEntityModel);
        }

        private ReactiveCollection<int> CreateTalentTree(ConfigTalentTreeContainer configTalentTreeContainer, ConfigTalentTreeEffectContainer configTalentTreeEffectContainer)
        {
            var talentTree = new ReactiveCollection<int>();
            talentTree.ObserveAdd()
                .Subscribe(x =>
                {
                    var talentTreeEffects = configTalentTreeEffectContainer.GroupConfigLookUp[x.Value];
                    foreach (var talentTreeEffect in talentTreeEffects)
                    {
                        var skillModel = HeroEntityModel.SkillModels.FirstOrDefault(skillModel => skillModel.ConfigSkill.ConfigSkillKey == talentTreeEffect.ConfigSkillKeyTarget);
                        switch (talentTreeEffect.TalentTreeEffectType)
                        {
                            case TalentTreeEffectType.ReduceCoolDownOfSkill:
                            {
                                skillModel.BuffCoolDown.Value += talentTreeEffect.Value;
                                break;
                            }
                            case TalentTreeEffectType.ReduceManaCostOfSkill:
                            {
                                skillModel.BuffManaCost.Value += talentTreeEffect.Value;
                                break;
                            }
                                
                            case TalentTreeEffectType.EnhanceSkillEffect:
                            {
                                var skillEffectModel = skillModel.SkillEffectModels[talentTreeEffect.ConfigSkillEffectTarget];
                                skillEffectModel.BuffEffectValue.Value += talentTreeEffect.Value;
                                break;
                            }
                            case TalentTreeEffectType.EnhanceSkillEffectDuration:
                            {
                                var skillEffectModel = skillModel.SkillEffectModels[talentTreeEffect.ConfigSkillEffectTarget];
                                skillEffectModel.BuffEffectDuration.Value += talentTreeEffect.Value;
                                break;
                            }
                            case TalentTreeEffectType.ChangeSkillCastType:
                            {
                                skillModel.SkillCastType.Value = ParseEnum<SkillCastType>(talentTreeEffect.StringValue);
                                break;
                            }
                            case TalentTreeEffectType.GrantMaxHp:
                                HeroEntityModel.MaxHp.Value += talentTreeEffect.Value;
                                break;
                            case TalentTreeEffectType.GrantMaxMana:
                                HeroEntityModel.MaxMana.Value += talentTreeEffect.Value;
                                break;
                            case TalentTreeEffectType.AddMoreProjectile:
                                skillModel.NumberProjectTile.Value += (int)talentTreeEffect.Value; 
                                break;
                            case TalentTreeEffectType.ChangeSkillEntity:
                                skillModel.SkillEntityPath = talentTreeEffect.StringValue;
                                break;
                            case TalentTreeEffectType.EnhanceAOESkill:
                                skillModel.BuffAoe.Value = talentTreeEffect.Value;
                                break;
                            case TalentTreeEffectType.AddSkillEffect:
                                skillModel.AddedSkillEffect.Add( new SkillEffectModel()
                                {
                                    SkillEffectType = ParseEnum<SkillEffectType>(talentTreeEffect.StringValue),
                                    EffectValue = new (talentTreeEffect.Value),
                                    EffectDuration = new(talentTreeEffect.Duration)
                                });
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                })
                .AddTo(this);
            return talentTree;
        }

        private static T ParseEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }

        private SkillModel CreateSkillModel(ConfigSkillContainer.Config config, ConfigSkillEffectContainer configSkillEffectContainer,
            ConfigSkillLevelContainer configSkillLevelContainer,
            ConfigSkillEffectLevelContainer configSkillEffectLevelContainer)
        {
            var skillModel = new SkillModel()
            {
                Level = new(0),
                ConfigSkill = config,
                SkillCastType = new(config.SkillCastType),
                SkillEffectModels = configSkillEffectContainer.GroupConfigLookUp[config.ConfigSkillKey].ToDictionary(skillEffectConfig => skillEffectConfig.ConfigSkillEffectKey, skillEffectConfig => new SkillEffectModel()
                {
                    SkillEffectType = skillEffectConfig.SkillEffectType,
                }).ToReactiveDictionary(),
                SkillEntityPath = config.SkillPrefabPath
            };

            
            skillModel
                .Level
                .Skip(1)
                .Subscribe(level =>
                {
                    var configSkillAtLevel = configSkillLevelContainer.GroupConfigLookUp[skillModel.ConfigSkill.ConfigSkillKey].ElementAt(level - 1);
                    skillModel.ConfigSkillAtLevel = configSkillAtLevel;
                    
                    CalculateSkillAttribute(skillModel);

                    foreach (var skillEffectModel in skillModel.SkillEffectModels)
                    {
                        var configSkillEffectAtLevel = configSkillEffectLevelContainer.GroupConfigLookUp[skillEffectModel.Key].ElementAt(level - 1);
                        skillEffectModel.Value.ConfigSkillEffectAtLevel = configSkillEffectAtLevel;
                        CalculateSkillEffectModel(skillEffectModel.Value);
                    }
                }).AddTo(this);

            skillModel.BuffAoe.Skip(1).Subscribe(_ =>
            {
                CalculateSkillAttribute(skillModel);
            }).AddTo(this);
            
            skillModel.BuffCastRange.Skip(1).Subscribe(_ =>
            {
                CalculateSkillAttribute(skillModel);
            }).AddTo(this);
            
            skillModel.BuffCoolDown.Skip(1).Subscribe(_ =>
            {
                CalculateSkillAttribute(skillModel);
            }).AddTo(this);
            
            skillModel.BuffManaCost.Skip(1).Subscribe(_ =>
            {
                CalculateSkillAttribute(skillModel);
            }).AddTo(this);
            
            foreach (var skillEffectModel in skillModel.SkillEffectModels)
            {
                skillEffectModel.Value.BuffEffectValue.Subscribe(_ => CalculateSkillEffectModel(skillEffectModel.Value)).AddTo(this);
                skillEffectModel.Value.BuffEffectDuration.Subscribe(_ => CalculateSkillEffectModel(skillEffectModel.Value)).AddTo(this);
                CalculateSkillEffectModel(skillEffectModel.Value);
            }
            
            return skillModel;
        }

        private static void CalculateSkillEffectModel(SkillEffectModel skillEffectModel)
        {
            if(skillEffectModel.ConfigSkillEffectAtLevel == default) return;
            skillEffectModel.EffectValue.Value = skillEffectModel.ConfigSkillEffectAtLevel.EffectValue * ( 1 + skillEffectModel.BuffEffectValue.Value);
            skillEffectModel.EffectDuration.Value = skillEffectModel.ConfigSkillEffectAtLevel.EffectDuration * ( 1 + skillEffectModel.BuffEffectValue.Value);
        }

        private static void CalculateSkillAttribute(SkillModel skillModel)
        {
            if (skillModel.ConfigSkillAtLevel == default) return;
            skillModel.Aoe.Value = skillModel.ConfigSkillAtLevel.Aoe * (1 + skillModel.BuffAoe.Value);
            skillModel.CastRange.Value = skillModel.ConfigSkillAtLevel.CastRange * (1 + skillModel.BuffCastRange.Value);
            skillModel.CoolDown.Value = skillModel.ConfigSkillAtLevel.CoolDown * (1 - skillModel.BuffCoolDown.Value);
            skillModel.ManaCost.Value = skillModel.ConfigSkillAtLevel.ManaCost * (1 - skillModel.BuffManaCost.Value);
        }

        private void Update()
        {
            HeroEntityModel.Hp.Value += HeroEntityModel.HpRegen.Value * Time.deltaTime;
            HeroEntityModel.Hp.Value = Mathf.Min(HeroEntityModel.Hp.Value, HeroEntityModel.MaxHp.Value);
            HeroEntityModel.Mana.Value += HeroEntityModel.HpRegen.Value * Time.deltaTime;
            HeroEntityModel.Mana.Value = Mathf.Min(HeroEntityModel.Mana.Value, HeroEntityModel.MaxMana.Value);
        }
    }
}