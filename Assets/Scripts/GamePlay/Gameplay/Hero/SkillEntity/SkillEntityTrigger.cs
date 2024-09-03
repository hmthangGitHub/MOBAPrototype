using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MobaPrototype.SkillEntity
{
    public class SkillEntityTrigger : MonoBehaviour
    {
        private Subject<IGetAttackAble> _onHitAttackAble;
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