using System;
using MobaPrototype.Config;
using UniRx;
using VContainer.Unity;

namespace MobaPrototype.SkillEntity
{
    public class SkillEntityTriggerPresenter : IInitializable, IDisposable
    {
        private readonly SkillEntityTrigger skillEntityTrigger;
        private readonly LifetimeScope scope;
        
        private SkillEntity SkillEntity => (SkillEntity)scope;
        private CompositeDisposable disposables = new();

        public SkillEntityTriggerPresenter(SkillEntityTrigger skillEntityTrigger, LifetimeScope scope)
        {
            this.skillEntityTrigger = skillEntityTrigger;
            this.scope = scope;
        }

        public void Initialize()
        {
            skillEntityTrigger.OnHitAttackAble.Subscribe(enemy =>
            {
                foreach (var effect in SkillEntity.SkillEffectModels)
                {
                    switch (effect.Key)
                    {
                        case SkillEffectType.Damage:
                            enemy.GetDamage(effect.Value.EffectValue);
                            break;
                        case SkillEffectType.Slow:
                            enemy.GetSlow(effect.Value.EffectValue, effect.Value.EffectDuration);
                            break;
                        case SkillEffectType.Stun:
                            enemy.GetStunned(effect.Value.EffectValue, effect.Value.EffectDuration);
                            break;
                        case SkillEffectType.DamagePerSecond:
                            enemy.GetDamagePerSecond(effect.Value.EffectValue, effect.Value.EffectDuration);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }).AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
            disposables = default;
        }
    }
}