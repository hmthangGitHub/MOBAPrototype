using System;
using Codice.Client.BaseCommands;
using DG.Tweening;
using MobaPrototype.Skills;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace MobaPrototype.Dummy
{
    public class Dummy : MonoBehaviour, IGetAttackAble
    {
        [SerializeField] private Transform[] wayPoints;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject stunnedEffect;

        private readonly int getHitHash = Animator.StringToHash("GetHit");
        private readonly int stunnedHash = Animator.StringToHash("Stunned");
        private readonly int movingHash = Animator.StringToHash("Moving");
        
        private int targetWayPointIndex = 0;
        private Tween tween;
        private float speed;

        private void Start()
        {
            speed = navMeshAgent.speed;
            navMeshAgent.destination = wayPoints[targetWayPointIndex].position;
            damageText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, wayPoints[targetWayPointIndex].position) <= 0.1f)
            {
                targetWayPointIndex++;
                targetWayPointIndex %= wayPoints.Length;
                navMeshAgent.destination = wayPoints[targetWayPointIndex].position;
            }
        }

        public void GetDamage(float valueEffectValue)
        {
            tween?.Kill(true);
            
            damageText.text = valueEffectValue.ToString();
            damageText.gameObject.SetActive(true);
            var damageTextTransform = ((RectTransform) damageText.transform);
            var currentY = damageTextTransform.anchoredPosition.y;
            tween = DOTween.Sequence()
                .Append(damageTextTransform.DOAnchorPosY(currentY + 20.0f, 0.2f).SetEase(Ease.OutElastic, 2.0f))
                .AppendInterval(1.0f)
                .OnComplete(() =>
            {
                damageText.gameObject.SetActive(false);
                damageTextTransform.anchoredPosition = new(damageTextTransform.anchoredPosition.x, currentY);
            });

            animator.Play(getHitHash);
        }

        public void GetSlow(float effectValue, float duration)
        {
            navMeshAgent.speed *= effectValue;
            Observable.Timer(TimeSpan.FromSeconds(duration))
                .Take(1)
                .Subscribe(_ =>
                {
                    navMeshAgent.speed /= effectValue;
                })
                .AddTo(this);
        }

        public void GetDamagePerSecond(float valueEffectValue, float valueEffectDuration)
        {
        }

        public void GetStunned(float duration)
        {
            navMeshAgent.speed = 0.0f;
            stunnedEffect.SetActive(true);
            animator.Play(stunnedHash);
            Observable.Timer(TimeSpan.FromSeconds(duration))
                .Take(1)
                .Subscribe(_ =>
                {
                    stunnedEffect.SetActive(false);
                    animator.Play(movingHash);
                    navMeshAgent.speed = speed;
                })
                .AddTo(this);
        }
    }
}