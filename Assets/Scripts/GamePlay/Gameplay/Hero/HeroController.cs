using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MobaPrototype.Config;
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
            builder.RegisterEntryPoint<HeroCommandExecutor>();
            builder.Register<HeroAoeSkillExecutor>(Lifetime.Singleton).AsSelf();
        }

        private void InitializeHeroEntityModel(IContainerBuilder builder)
        {
            var configHeroContainer = Parent.Container.Resolve<ConfigHeroContainer>();
            var configSkillContainer = Parent.Container.Resolve<ConfigSkillContainer>();
            var configSkillLevelContainer = Parent.Container.Resolve<ConfigSkillLevelContainer>();
            HeroEntityModel = new()
            {
                Level = new(1),
                TalentTree = new(2),
                HeroConfig = configHeroContainer.ConfigDictionary[ConfigHeroKey],
                SkillModels = configSkillContainer.GroupConfigLookUp[ConfigHeroKey].Select(x =>
                {
                    var skillModel = new SkillModel()
                    {
                        Level = new(0),
                        ConfigSkill = x,
                        SkillCastType = new(x.SkillCastType)
                    };

                    skillModel
                        .Level
                        .Skip(1)
                        .Subscribe(level =>
                        {
                            var configSkillAtLevel = configSkillLevelContainer.GroupConfigLookUp[skillModel.ConfigSkill.ConfigSkillKey].ElementAt(level - 1);
                            skillModel.Aoe.Value = configSkillAtLevel.Aoe;
                            skillModel.CastRange.Value = configSkillAtLevel.CastRange;
                            skillModel.CoolDown.Value = configSkillAtLevel.CoolDown;
                            skillModel.ManaCost.Value = configSkillAtLevel.ManaCost;
                        }).AddTo(this);
                    return skillModel;
                }).ToArray(),
            };
            builder.RegisterInstance(HeroEntityModel);
        }
    }
}