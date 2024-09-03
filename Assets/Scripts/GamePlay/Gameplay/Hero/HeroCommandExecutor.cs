using System;
using System.Collections.Generic;
using System.Linq;
using MobaPrototype.Config;
using UniRx;
using VContainer.Unity;

namespace MobaPrototype.Hero
{
    public class HeroCommandExecutor : IInitializable, IDisposable
    {
        private readonly HeroCommand heroCommand;
        private readonly HeroEntityModel heroEntityModel;
        private readonly HeroAoeSkillExecutor heroAoeSkillExecutor;

        private CompositeDisposable compositeDisposable = new();
        
        public HeroCommandExecutor(HeroCommand heroCommand, HeroEntityModel heroEntityModel, HeroAoeSkillExecutor heroAoeSkillExecutor)
        {
            this.heroCommand = heroCommand;
            this.heroEntityModel = heroEntityModel;
            this.heroAoeSkillExecutor = heroAoeSkillExecutor;
        }
        
        public void Initialize()
        {
            heroCommand.SkillCastingCommand.Subscribe(x =>
            {
                var heroSkillExecutor = GetHeroAoeSkillExecutor(x.SkillIndex);
                heroSkillExecutor.Execute(x);
            }).AddTo(compositeDisposable);
            
            heroCommand.SkillPreviewCommand.Subscribe(x =>
            {
                var heroSkillExecutor = GetHeroAoeSkillExecutor(x.SkillIndex);
                heroSkillExecutor.Preview(x);
            }).AddTo(compositeDisposable);
            
            heroCommand.SkillPreviewExitCommand.Subscribe(x =>
            {
                var heroSkillExecutor = GetHeroAoeSkillExecutor(x.SkillIndex);
                heroSkillExecutor.ExitPreview(x);
            }).AddTo(compositeDisposable);
        }

        private HeroAoeSkillExecutor GetHeroAoeSkillExecutor(int skillIndex)
        {
            var heroSkillExecutor = heroEntityModel.SkillModels[skillIndex].SkillCastType.Value switch
            {
                SkillCastType.Direction => default,
                SkillCastType.Target => default,
                SkillCastType.NoneTarget => heroAoeSkillExecutor,
                _ => throw new ArgumentOutOfRangeException()
            };
            return heroSkillExecutor;
        }

        public void Dispose()
        {
            compositeDisposable.Dispose();
            compositeDisposable = default;
        }
    }
}