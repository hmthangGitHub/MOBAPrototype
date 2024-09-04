using System;
using System.Linq;
using MobaPrototype.Scope;
using MobaPrototype.Skills;
using UniRx;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace MobaPrototype.Hero
{
    public class HeroAoeSkillExecutor : HeroSkillExecutorBase
    {
        protected override void OnExecute(SkillModel skillModel)
        {
            base.OnExecute(skillModel);
            var skillEntity = gameObjectPoolContainer.GetObject<AoeSkillEntity>(skillModel.ConfigSkill.SkillPrefabPath);
            skillEntity.SetModel(new ()
            {
                Aoe = skillModel.Aoe.Value,
                SkillEntityModel = new SkillEntityModel()
                {
                    SkillEffectModels = skillModel.SkillEffectModels.Values.ToArray()
                },
                Position = HeroController.transform.position
            });
        }

        protected override float GetPreviewRange(SkillModel skillModel)
        {
            return skillModel.Aoe.Value;
        }
    }
}