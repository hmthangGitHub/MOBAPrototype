using System;
using System.Linq;
using DG.Tweening;
using MobaPrototype.Skills;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Hero
{
    public class HeroTargetSkillExecutor : HeroSkillExecutorBase
    {
        private IDisposable castingSkillToTargetDisposable;
        private ITargetAble currentTarget;
        
        public override void Preview(int skillIndex)
        {
            if (!ValidateSkillIndex(skillIndex)) return;
            var skillModel = heroEntityModel.SkillModels[skillIndex];
            targetSkillPreviewer.Enable = true;
            aoeSkillPreviewer.Enable = true;
            aoeSkillPreviewer.Aoe = GetPreviewRange(skillModel);

            castingSkillToTargetDisposable?.Dispose();
            castingSkillToTargetDisposable = targetSkillPreviewer
                .OnCastingSkillToTarget
                .Take(1)
                .Subscribe(x =>
                {
                    currentTarget = x.target;
                    ExitPreview();
                    RotateToTargetAndExecuteSkillIndex(skillIndex, x.direction);
                }).AddTo(disposables);
        }
        
        protected override float GetPreviewRange(SkillModel skillModel) => skillModel.CastRange.Value * 2.0f;


        protected override void OnExecute(SkillModel skillModel)
        {
            currentTarget.ApplySkillEffectToTarget(skillModel.SkillEffectModels.Values.ToArray());
        }
    }
}