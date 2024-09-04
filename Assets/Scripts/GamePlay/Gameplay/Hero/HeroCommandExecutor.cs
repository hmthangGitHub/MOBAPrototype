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
        private readonly HeroDirectionalSkillExecutor heroDirectionalSkillExecutor;
        private readonly HeroTargetSkillExecutor heroTargetSkillExecutor;

        private CompositeDisposable compositeDisposable = new();

        public HeroCommandExecutor(HeroCommand heroCommand, HeroEntityModel heroEntityModel,
            HeroAoeSkillExecutor heroAoeSkillExecutor, HeroDirectionalSkillExecutor heroDirectionalSkillExecutor,
            HeroTargetSkillExecutor heroTargetSkillExecutor)
        {
            this.heroCommand = heroCommand;
            this.heroEntityModel = heroEntityModel;
            this.heroAoeSkillExecutor = heroAoeSkillExecutor;
            this.heroDirectionalSkillExecutor = heroDirectionalSkillExecutor;
            this.heroTargetSkillExecutor = heroTargetSkillExecutor;
        }

        public void Initialize()
        {
            heroCommand.SkillCastingCommand.Subscribe(x =>
            {
                var heroSkillExecutor = GetSkillExecutor(x.SkillIndex);
                heroSkillExecutor.Execute(x.SkillIndex);
            }).AddTo(compositeDisposable);

            heroCommand.SkillPreviewCommand.Subscribe(x =>
            {
                var heroSkillExecutor = GetSkillExecutor(x.SkillIndex);
                heroSkillExecutor.Preview(x.SkillIndex);
            }).AddTo(compositeDisposable);

            heroCommand.SkillPreviewRangeCommand.Subscribe(x =>
            {
                heroDirectionalSkillExecutor.ExitPreview();
                heroAoeSkillExecutor.ExitPreview();
                var heroSkillExecutor = GetSkillExecutor(x.SkillIndex);
                heroSkillExecutor.PreviewRange(x.SkillIndex);
            }).AddTo(compositeDisposable);

            heroCommand.SkillPreviewExitCommand.Subscribe(x =>
            {
                var heroSkillExecutor = GetSkillExecutor(x.SkillIndex);
                heroSkillExecutor.ExitPreview();
            }).AddTo(compositeDisposable);
        }

        private IHeroSkillExecutor GetSkillExecutor(int skillIndex)
        {
            return heroEntityModel.SkillModels[skillIndex].SkillCastType.Value switch
            {
                SkillCastType.Direction => heroDirectionalSkillExecutor,
                SkillCastType.Aoe => heroAoeSkillExecutor,
                SkillCastType.Target => heroTargetSkillExecutor,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Dispose()
        {
            compositeDisposable.Dispose();
            compositeDisposable = default;
        }
    }
}