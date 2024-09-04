using System;
using System.Linq;
using MobaPrototype.Scope;
using MobaPrototype.Skills;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype.Hero
{
    public class HeroSkillExecutorBase : IHeroSkillExecutor
    {
        [Inject] private LifetimeScope scope;
        [Inject] protected HeroEntityModel heroEntityModel;
        [Inject] protected CharacterAnimatorController characterAnimatorController;
        [Inject] protected AOESkillPreviewer aoeSkillPreviewer;
        [Inject] protected RangeSkillPreviewer rangeSkillPreviewer;
        [Inject] protected GameObjectPoolContainer gameObjectPoolContainer;
        
        protected IDisposable onSkillActivateDisposable;
        protected CompositeDisposable disposables = new();
        protected HeroController HeroController => (HeroController)scope;

        protected bool ValidateSkillIndex(int index)
        {
            var skillModel = heroEntityModel.SkillModels[index];
            return skillModel.Level.Value > 0 ;
        }
        
        protected virtual float GetPreviewRange(SkillModel skillModel) => 0.0f;

        public void ExitPreview()
        {
            aoeSkillPreviewer.Enable = false;
            rangeSkillPreviewer.Enable = false;
        }

        protected virtual void OnExecute(SkillModel skillModel)
        {
        }

        public void Execute(int skillIndex)
        {
            if (!ValidateSkillIndex(skillIndex)) return;
            var skillModel = heroEntityModel.SkillModels[skillIndex];
            characterAnimatorController.PlaySkillAnimation(skillIndex);
            onSkillActivateDisposable?.Dispose();
            onSkillActivateDisposable = characterAnimatorController.OnSkillActivate.Take(1).Subscribe(_ =>
            {
                OnExecute(skillModel);
            }).AddTo(disposables);
        }

        public void PreviewRange(int skillIndex)
        {
            if (!ValidateSkillIndex(skillIndex)) return;
            var skillModel = heroEntityModel.SkillModels[skillIndex];
            aoeSkillPreviewer.Enable = true;
            aoeSkillPreviewer.Aoe = GetPreviewRange(skillModel);
        }

        public virtual void Preview(int skillIndex)
        {
        }
        
        public void Dispose()
        {
            disposables.Dispose();
            disposables = default;
        }
    }
}