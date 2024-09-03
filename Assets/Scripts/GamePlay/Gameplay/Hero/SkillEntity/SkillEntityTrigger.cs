using System;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Skills
{
    public class SkillEntityTrigger : MonoBehaviour
    {
        private Subject<IGetAttackAble> _onHitAttackAble = new();
        public IObservable<IGetAttackAble> OnHitAttackAble => _onHitAttackAble;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IGetAttackAble>(out var attackAble))
            {
                _onHitAttackAble.OnNext(attackAble);
            }
        }
    }
}