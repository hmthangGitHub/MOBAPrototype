using System;
using System.Linq;
using DG.Tweening;
using MobaPrototype.Skills;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Hero
{
    public class HeroDirectionalSkillExecutor : HeroSkillExecutorBase
    {
        private IDisposable castingDirectionDisposable;
        protected override float GetPreviewRange(SkillModel skillModel)
        {
            return skillModel.CastRange.Value * 2.0f;
        }
        
        public override void Preview(int skillIndex)
        {
            if (!ValidateSkillIndex(skillIndex)) return;
            var skillModel = heroEntityModel.SkillModels[skillIndex];
            rangeSkillPreviewer.Range = skillModel.CastRange.Value;
            rangeSkillPreviewer.Enable = true;
            aoeSkillPreviewer.Enable = true;
            aoeSkillPreviewer.Aoe = GetPreviewRange(skillModel);

            castingDirectionDisposable?.Dispose();
            castingDirectionDisposable = rangeSkillPreviewer
                .CastingDirection
                .Take(1)
                .Subscribe(direction =>
                {
                    ExitPreview();
                    RotateToTargetAndExecuteSkillIndex(skillIndex, direction);
                }).AddTo(disposables);
        }
        
        protected override void OnExecute(SkillModel skillModel)
        {
            base.OnExecute(skillModel);
            var skillEntity = gameObjectPoolContainer.GetObject<DirectionalSkillEntity>(skillModel.ConfigSkill.SkillPrefabPath);
            skillEntity.SetModel(new ()
            {
                SkillEntityModel = new()
                {
                    SkillEffectModels = skillModel.SkillEffectModels.Values.ToArray()
                },
                Direction = HeroController.transform.forward,
                Range = skillModel.CastRange.Value / 100.0f,
                Position = HeroController.transform.position
            });
        }
    }
}