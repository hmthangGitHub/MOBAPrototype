using System;
using System.Linq;
using MobaPrototype.Scope;
using MobaPrototype.Skills;
using UniRx;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype.Hero
{
    public class HeroAoeSkillExecutor : IHeroSkillExecutor, IDisposable
    {
        private readonly HeroEntityModel heroEntityModel;
        private readonly CharacterAnimatorController characterAnimatorController;
        private readonly AOESkillPreviewer aoeSkillPreviewer;
        private readonly GameObjectPoolContainer gameObjectPoolContainer;
        private readonly LifetimeScope scope;

        private HeroController HeroController => (HeroController)scope;
        private IDisposable onSkillActivateDisposable;
        private CompositeDisposable compositeDisposable = new();

        [Inject]
        public HeroAoeSkillExecutor(HeroEntityModel heroEntityModel, AOESkillPreviewer aoeSkillPreviewer,
            CharacterAnimatorController characterAnimatorController, GameObjectPoolContainer gameObjectPoolContainer, LifetimeScope scope)
        {
            this.heroEntityModel = heroEntityModel;
            this.aoeSkillPreviewer = aoeSkillPreviewer;
            this.characterAnimatorController = characterAnimatorController;
            this.gameObjectPoolContainer = gameObjectPoolContainer;
            this.scope = scope;
        }

        public void Execute(SkillCastingCommand skillCastingCommand)
        {
            var skillModel = heroEntityModel.SkillModels[skillCastingCommand.SkillIndex];
            if (skillModel.Level.Value <= 0) return;
            characterAnimatorController.PlaySkillAnimation(skillCastingCommand.SkillIndex);
            onSkillActivateDisposable?.Dispose();
            onSkillActivateDisposable = characterAnimatorController.OnSkillActivate.Subscribe(_ =>
            {
                var skillEntity = gameObjectPoolContainer.GetObject<SkillEntity>(skillModel.ConfigSkill.SkillPrefabPath);
                skillEntity.SkillEffectModels = skillModel.SkillEffectModels.Values.ToArray();
                skillEntity.SetAoe(skillModel.Aoe.Value);
                skillEntity.transform.position = HeroController.transform.position;
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