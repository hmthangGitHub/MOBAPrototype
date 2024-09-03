using System;
using UniRx;
using VContainer;

namespace MobaPrototype.Hero
{
    public class HeroAoeSkillExecutor : IHeroSkillExecutor, IDisposable
    {
        private readonly HeroEntityModel heroEntityModel;
        private readonly CharacterAnimatorController characterAnimatorController;
        private readonly AOESkillPreviewer aoeSkillPreviewer;
        
        private IDisposable onSkillActivateDisposable;
        private CompositeDisposable compositeDisposable = new();

        [Inject]
        public HeroAoeSkillExecutor(HeroEntityModel heroEntityModel, AOESkillPreviewer aoeSkillPreviewer, CharacterAnimatorController characterAnimatorController)
        {
            this.heroEntityModel = heroEntityModel;
            this.aoeSkillPreviewer = aoeSkillPreviewer;
            this.characterAnimatorController = characterAnimatorController;
        }

        public void Execute(SkillCastingCommand skillCastingCommand)
        {
            characterAnimatorController.PlaySkillAnimation(skillCastingCommand.SkillIndex);
            onSkillActivateDisposable?.Dispose();
            onSkillActivateDisposable = characterAnimatorController.OnSkillActivate.Subscribe(_ =>
            {
                
            }).AddTo(compositeDisposable);
        }

        public void Preview(SkillPreviewCommand skillPreviewCommand)
        {
            var skillModel = heroEntityModel.SkillModels[skillPreviewCommand.SkillIndex];
            if (skillModel.Level.Value <= 0) return;
            aoeSkillPreviewer.Enable = true;
            aoeSkillPreviewer.Aoe = skillModel.Aoe.Value;
        }

        public void ExitPreview(SkillPreviewExitCommand skillPreviewExitCommand)
        {
            var skillModel = heroEntityModel.SkillModels[skillPreviewExitCommand.SkillIndex];
            if (skillModel.Level.Value <= 0) return;
            aoeSkillPreviewer.Enable = false;
        }

        public void Dispose()
        {
            compositeDisposable.Dispose();
            compositeDisposable = default;
        }
    }
}