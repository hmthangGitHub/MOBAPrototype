using System;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Skills
{
    public class SkillEntityTrigger : MonoBehaviour
    {
        private Subject<ITargetAble> _onHitAttackAble = new();
        public IObservable<ITargetAble> OnHitAttackAble => _onHitAttackAble;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ITargetAble>(out var attackAble))
            {
                _onHitAttackAble.OnNext(attackAble);
            }
        }
    }
}