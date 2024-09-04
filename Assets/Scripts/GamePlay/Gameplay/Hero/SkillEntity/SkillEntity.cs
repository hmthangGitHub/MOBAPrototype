using System;
using System.Collections.Generic;
using MobaPrototype.Config;
using MobaPrototype.Hero;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Skills
{
    public class SkillEntity : MonoBehaviour
    {
        [SerializeField] private SkillEntityTrigger skillEntityTrigger;
        [SerializeField] private GameObjectEventPool gameObjectEventPool;

        public SkillEntityModel SkillEntityModel { get; set; }

        private void Start()
        {
            skillEntityTrigger.OnHitAttackAble.Subscribe(target => target.ApplySkillEffectToTarget(SkillEntityModel.SkillEffectModels)).AddTo(this);
        }

        public void OnSKillEntityEnd()
        {
            gameObjectEventPool.ReturnToPool();
        }
    }

    public class SkillEntityModel
    {
        public SkillEffectModel[] SkillEffectModels { get; set; }
    }
}