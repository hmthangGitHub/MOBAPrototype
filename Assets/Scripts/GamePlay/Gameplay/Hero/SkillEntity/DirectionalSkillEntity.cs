using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace MobaPrototype.Skills
{
    public class DirectionalSkillEntity : MonoBehaviour
    {
        public class DirectionalSkillEntityModel
        {
            public SkillEntityModel SkillEntityModel { get; set; }
            public float Range { get; set; }
            public Vector3 Direction { get; set; }
            public Vector3 Position { get; set; }
        }

        [SerializeField] private SkillEntity skillEntity;
        [SerializeField] private GameObjectEventPool gameObjectEventPool;
        [SerializeField] private SkillEntityTrigger skillEntityTrigger;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private Vector3 offset;
        private IDisposable onHitAttackDisposable;
        
        private float travelTime;
        private float currentTravelTime;

        public void SetModel(DirectionalSkillEntityModel directionalSkillEntityModel)
        {
            skillEntity.SkillEntityModel = directionalSkillEntityModel.SkillEntityModel;
            travelTime = (directionalSkillEntityModel.Range - offset.z)/ speed;
            transform.forward = directionalSkillEntityModel.Direction;
            transform.position = directionalSkillEntityModel.Position + transform.TransformDirection(offset);
            
            onHitAttackDisposable?.Dispose();
            onHitAttackDisposable = skillEntityTrigger
                .OnHitAttackAble
                .Take(1)
                .Subscribe(_ =>
                {
                    gameObjectEventPool.ReturnToPool();
                })
                .AddTo(this);
        }

        private void Update()
        {
            currentTravelTime += Time.deltaTime;
            transform.position += speed * transform.forward * Time.deltaTime;
            if (currentTravelTime >= travelTime)
            {
                gameObjectEventPool.ReturnToPool();
            }
        }

        private void OnEnable()
        {
            currentTravelTime = 0.0f;
        }
    }
}