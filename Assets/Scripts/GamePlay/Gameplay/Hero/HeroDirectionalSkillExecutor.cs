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
        private Vector3[] projectileDirections;

        protected override float GetPreviewRange(SkillModel skillModel)
        {
            return skillModel.CastRange.Value * 2.0f;
        }
        
        public override void Preview(int skillIndex)
        {
            base.Preview(skillIndex);
            if (!ValidateSkillIndex(skillIndex)) return;
            var skillModel = heroEntityModel.SkillModels[skillIndex];
            rangeSkillPreviewer.Range = skillModel.CastRange.Value;
            rangeSkillPreviewer.NumberOfProjectile = skillModel.NumberProjectTile.Value;
            rangeSkillPreviewer.Enable = true;
            aoeSkillPreviewer.Enable = true;
            aoeSkillPreviewer.Aoe = GetPreviewRange(skillModel);

            castingDirectionDisposable?.Dispose();
            castingDirectionDisposable = rangeSkillPreviewer
                .CastingDirection
                .Take(1)
                .Subscribe(direction =>
                {
                    projectileDirections = direction.projectTileDirection;
                    ExitPreview();
                    RotateToTargetAndExecuteSkillIndex(skillIndex, direction.heroDirection);
                }).AddTo(disposables);
        }
        
        protected override void OnExecute(SkillModel skillModel)
        {
            base.OnExecute(skillModel);

            foreach (var projectileDirection in projectileDirections)
            {
                var skillEntity = gameObjectPoolContainer.GetObject<DirectionalSkillEntity>(skillModel.SkillEntityPath);
                skillEntity.SetModel(new ()
                {
                    SkillEntityModel = new()
                    {
                        SkillEffectModels = skillModel.AllSkillEffectModels
                    },
                    Direction = projectileDirection,
                    Range = skillModel.CastRange.Value / 100.0f,
                    Position = HeroController.transform.position
                });
            }
        }
    }
}