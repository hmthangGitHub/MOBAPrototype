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
        [SerializeField] private Animator animator;
        [SerializeField] private SkillEntityTrigger skillEntityTrigger;
        [SerializeField] private GameObjectEventPool gameObjectEventPool;
        [SerializeField] private Transform skillAoeContainer;
        public SkillEffectModel[] SkillEffectModels { get; set; }

        private void Start()
        {
            skillEntityTrigger.OnHitAttackAble.Subscribe(enemy =>
            {
                foreach (var effect in SkillEffectModels)
                {
                    switch (effect.SkillEffectType)
                    {
                        case SkillEffectType.Damage:
                            enemy.GetDamage(effect.EffectValue);
                            break;
                        case SkillEffectType.Slow:
                            enemy.GetSlow(effect.EffectValue, effect.EffectDuration);
                            break;
                        case SkillEffectType.Stun:
                            enemy.GetStunned(effect.EffectValue, effect.EffectDuration);
                            break;
                        case SkillEffectType.DamagePerSecond:
                            enemy.GetDamagePerSecond(effect.EffectValue, effect.EffectDuration);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }).AddTo(this);
        }

        public void SetAoe(float value)
        {
            skillAoeContainer.localScale = Vector3.one * value / 100.0f;
        }

        public void OnSKillEntityEnd()
        {
            gameObjectEventPool.ReturnToPool();
        }
    }

    public class SkillEntityModel
    {
        public IReadOnlyDictionary<SkillEffectType, SkillEffectModel> SkillEffectModels { get; set; }
    }
}