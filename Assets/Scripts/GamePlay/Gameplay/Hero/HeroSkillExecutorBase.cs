using System;
using System.Linq;
using DG.Tweening;
using MobaPrototype.Scope;
using MobaPrototype.SkillEntity;
using MobaPrototype.Skills;
using UniRx;
using UnityEngine;
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
        [Inject] protected TargetSkillPreviewer targetSkillPreviewer;
        [Inject] protected GameObjectPoolContainer gameObjectPoolContainer;
        
        protected IDisposable onSkillActivateDisposable;
        protected CompositeDisposable disposables = new();
        protected HeroController HeroController => (HeroController)scope;

        protected bool ValidateSkillIndex(int index)
        {
            var skillModel = heroEntityModel.SkillModels[index];
            return skillModel.Level.Value > 0;
        }
        
        protected virtual float GetPreviewRange(SkillModel skillModel) => 0.0f;

        public void ExitPreview()
        {
            aoeSkillPreviewer.Enable = false;
            rangeSkillPreviewer.Enable = false;
            targetSkillPreviewer.Enable = false;
        }

        protected virtual void OnExecute(SkillModel skillModel)
        {
        }

        public void Execute(int skillIndex)
        {
            if (!ValidateSkillIndex(skillIndex)) return;
            var skillModel = heroEntityModel.SkillModels[skillIndex];
            if (skillModel.ManaCost.Value > heroEntityModel.Mana.Value || skillModel.CoolDownTimeStamp.Value > Time.time) return;
            heroEntityModel.Mana.Value -= skillModel.ManaCost.Value;
            heroEntityModel.Mana.Value = Mathf.Max(0, heroEntityModel.Mana.Value);
            skillModel.CoolDownTimeStamp.Value = Time.time + skillModel.CoolDown.Value; 
            
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

        protected void RotateToTargetAndExecuteSkillIndex(int skillIndex, Vector3 targetDirection)
        {
            HeroController.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(targetDirection), 0.2f)
                .OnComplete(() =>
                {
                    Execute(skillIndex);
                });
        }
        
        public void Dispose()
        {
            disposables.Dispose();
            disposables = default;
        }
    }
}