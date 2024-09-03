using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Hero
{
    public class CharacterAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Subject<Unit> onSkillActivate = new();

        private int[] skillAnimationHash = new[]
        {
            Animator.StringToHash($"Skill1"),
            Animator.StringToHash($"Skill2"),
            Animator.StringToHash($"Skill3"),
            Animator.StringToHash($"Skill4"),
            Animator.StringToHash($"Skill5"),
            Animator.StringToHash($"Skill6"),
        };
        
        public IObservable<Unit> OnSkillActivate => onSkillActivate;

        public void PlaySkillAnimation(int skillIndex)
        {
            animator.CrossFade(skillAnimationHash[skillIndex], 0.1f);
        }

        public void ActivateSkill() => onSkillActivate.OnNext(default);
    }
}