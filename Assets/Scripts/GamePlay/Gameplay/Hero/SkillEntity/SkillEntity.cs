using System;
using System.Collections;
using System.Collections.Generic;
using MobaPrototype.Config;
using MobaPrototype.Hero;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype.SkillEntity
{
    public class SkillEntity : LifetimeScope
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SkillEntityTrigger skillEntityTrigger;
        public IReadOnlyDictionary<SkillEffectType, SkillEffectModel> SkillEffectModels { get; set; }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterInstance(skillEntityTrigger);
            builder.RegisterEntryPoint<SkillEntityTriggerPresenter>();
        }

        private void OnEnable()
        {
            skillEntityTrigger.OnHitAttackAble.Subscribe(enemy =>
            {
            }).AddTo(this);
        }

        public void OnSKillEntityEnd()
        {
            Destroy(gameObject);
        }
    }

    public class SkillEntityModel
    {
        public IReadOnlyDictionary<SkillEffectType, SkillEffectModel> SkillEffectModels { get; set; }
    }
}