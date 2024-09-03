using System.Collections;
using System.Collections.Generic;
using MobaPrototype.SkillEntity;
using UnityEngine;

namespace MobaPrototype.Dummy
{
    public class Dummy : MonoBehaviour, IGetAttackAble
    {
        public void GetDamage(float valueEffectValue)
        {
        }

        public void GetSlow(float effectValue, float valueEffectValue)
        {
        }

        public void GetDamagePerSecond(float valueEffectValue, float valueEffectDuration)
        {
        }

        public void GetStunned(float valueEffectValue, float valueEffectDuration)
        {
        }
    }
}